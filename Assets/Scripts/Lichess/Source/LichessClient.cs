using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class LichessClient : MonoBehaviour, ILichessClient
{
    public bool DisableLogin = false;

    private ConcurrentQueue<LichessMessage> messageQueue = new ConcurrentQueue<LichessMessage>();

    private Thread backgroundThread;

    private string guiText = ":-)";

    private ILichessLoginRetriever lichessLoginRetriever;

    private LichessLogin lichessLogin;

    private LichessMessage? lastGameStateMessage = null;

    public bool TryGetMessage(out LichessMessage message)
    {
        return this.messageQueue.TryDequeue(out message);
    }

    public async void SendMove(string move)
    {
        if (this.DisableLogin
            || this.lichessLoginRetriever == null)
        {
            return;
        }

        Debug.Log($"Sending move request {move}...");
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.lichessLogin.ApiKey);
        var response = await client.PostAsync($"https://lichess.org/api/board/game/{this.lichessLogin.GameId}/move/{move}", content: null);
        Debug.Log($"Response: {response.StatusCode}");

        var moveMessage = new LichessMessage
        {
            MessageType = LichessMessageType.MoveResponse,
            MoveResponseMessage = new MoveResponseMessage
            {
                IsSuccess = response.StatusCode == HttpStatusCode.OK
            }
        };
        this.messageQueue.Enqueue(moveMessage);
        this.guiText = $"[{DateTime.Now}]: Move " + (moveMessage.MoveResponseMessage.Value.IsSuccess ? "succeded!" : "failed!");
    }

    public void Start()
    {
        this.lichessLoginRetriever = GameObject.Find("MainMenu")?.GetComponent<ILichessLoginRetriever>();
    }

    public void Update()
    {
        if (this.DisableLogin
            || this.lichessLoginRetriever == null
            || !this.lichessLoginRetriever.TryGetLichessLogin(out this.lichessLogin))
        {
            // Wait until we have a login
            return;
        }

        if (this.backgroundThread == null)
        {
            this.backgroundThread = new Thread(this.ReadDataStream);
            this.backgroundThread.Start();
        }
    }

    void OnGUI()
    {
        var guiStyle = new GUIStyle
        {
            normal = new GUIStyleState
            {
                textColor = Color.black
            }
        };
        GUI.Label(new Rect(0, 0, 200, 50), this.guiText, guiStyle);
    }

    private async void ReadDataStream()
    {
        try
        {
            Debug.Log($"Login: {JsonConvert.SerializeObject(this.lichessLogin)}");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.lichessLogin.ApiKey);

            var json = string.Empty;
            using (var stream = await client.GetStreamAsync($"https://lichess.org/api/board/game/stream/{this.lichessLogin.GameId}"))
            {
                var line = new List<char>();
                var buffer = new byte[1];
                while (true)
                {
                    var nextByte = stream.ReadByte();
                    if (nextByte == -1)
                    {
                        break;
                    }

                    line.Add(Char.ConvertFromUtf32(nextByte)[0]);
                    if (nextByte == '\n')
                    {
                        json = string.Join("", line);
                        line.Clear();

                        if (json.Length == 1)
                        {
                            continue;
                        }

                        LichessMessage message;
                        var jObject = JObject.Parse(json);
                        var messageType = jObject["type"].Value<string>();
                        switch (messageType)
                        {
                            case "gameFull":
                                message = this.ParseFullGameMessage(json);
                                lastGameStateMessage = message;
                                break;
                            case "gameState":
                                message = this.ParseGameStateMessage(json);
                                lastGameStateMessage = message;
                                break;
                            case "chatLine":
                                message = this.ParseChatLineMessage(json);
                                break;
                            default:
                                throw new Exception($"Unhandled Lichess message type: {messageType}!");

                        }

                        this.guiText = $"[{DateTime.Now}]: Successfully read Lichess message!";
                        this.messageQueue.Enqueue(message);
                    }
                }
            }
        }
        catch (Exception e)
        {
            this.guiText = $"[{DateTime.Now}]: Exception: {e}";
        }
    }

    private LichessMessage ParseFullGameMessage(string json)
    {
        return new LichessMessage
        {
            MessageType = LichessMessageType.GameFull,
            FullGameUpdateMessage = JsonConvert.DeserializeObject<FullGameUpdateMessage>(json)
        };
    }

    private LichessMessage ParseGameStateMessage(string json)
    {
        return new LichessMessage
        {
            MessageType = LichessMessageType.GameState,
            GameStateMessage = JsonConvert.DeserializeObject<GameStateMessage>(json)
        };
    }

    private LichessMessage ParseChatLineMessage(string json)
    {
        return new LichessMessage
        {
            MessageType = LichessMessageType.ChatLine,
            ChatLineMessage = JsonConvert.DeserializeObject<ChatLineMessage>(json)
        };
    }
}