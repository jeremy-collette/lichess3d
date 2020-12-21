using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class LichessClient : MonoBehaviour, ILichessClient
{
    private ConcurrentQueue<LichessMessage> messageQueue = new ConcurrentQueue<LichessMessage>();

    private Thread backgroundThread;

    private Exception backgroundThreadException = null;

    private ILichessLoginRetriever lichessLoginRetriever;

    private LichessLogin lichessLogin;

    public bool TryGetMessage(out LichessMessage message)
    {
        return this.messageQueue.TryDequeue(out message);
    }

    public void Start()
    {
        this.lichessLoginRetriever = GameObject.Find("MainMenu").GetComponent<ILichessLoginRetriever>();
    }

    public void Update()
    {
        if (!this.lichessLoginRetriever.TryGetLichessLogin(out this.lichessLogin))
        {
            // Wait until we have a login
            return;
        }

        if (this.backgroundThread == null)
        {
            this.backgroundThread = new Thread(this.ReadDataStream);
            this.backgroundThread.Start();
        }

        if (this.backgroundThreadException != null)
        {
            Debug.LogError(this.backgroundThreadException);
            //throw this.backgroundThreadException;
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
        GUI.Label(new Rect(0, 0, 200, 50), this.backgroundThreadException?.ToString() ?? ":-)", guiStyle);
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
                                break;
                            case "gameState":
                                message = this.ParseGameStateMessage(json);
                                break;
                            case "chatLine":
                                message = this.ParseChatLineMessage(json);
                                break;
                            default:
                                throw new Exception($"Unhandled Lichess message type: {messageType}!");

                        }

                        this.messageQueue.Enqueue(message);
                    }
                }
            }
        }
        catch (Exception e)
        {
            this.backgroundThreadException = e;
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