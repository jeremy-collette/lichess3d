using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LichessBoardStateReader : MonoBehaviour, IBoardStateReader
{
    private ILichessClient lichessClient;

    private List<BoardMove> lastMoves = new List<BoardMove>();

    public void Start()
    {
        this.lichessClient = this.GetComponent<ILichessClient>();
    }

    public bool TryGetBoardState(out BoardStateInput gameInputUpdate)
    {
        gameInputUpdate = new BoardStateInput { };
        if (!this.lichessClient.TryGetMessage(out var message))
        {
            return false;
        }

        var moves = new List<BoardMove>();
        if (message.FullGameUpdateMessage.HasValue)
        {
            Debug.Log($"Got Lichess FullGameUpdate: {message.FullGameUpdateMessage.Value.State.Moves}");
            moves = this.GetBoardMoves(message.FullGameUpdateMessage.Value.State.Moves).ToList();
        }
        else if (message.GameStateMessage.HasValue)
        {
            Debug.Log($"Got Lichess GameState: {message.GameStateMessage.Value.Moves}");
            moves = this.GetBoardMoves(message.GameStateMessage.Value.Moves).ToList();
        }
        else if (message.MoveResponseMessage.HasValue)
        {
            Debug.Log($"Got Lichess MoveResponse: IsSuccess={message.MoveResponseMessage.Value.IsSuccess}");
            // If the move succeeded, we don't need to do anything
            if (message.MoveResponseMessage.Value.IsSuccess)
            {
                return false;
            }
            else
            {
                // If the move failed, revert to previous state.
                Debug.LogWarning("Move failed! Reverting...");
                moves = lastMoves;
            }
        }

        gameInputUpdate.Moves = moves;

        // Store the last moves in case we need to revert
        lastMoves = gameInputUpdate.Moves;
        return true;
    }

    private IEnumerable<BoardMove> GetBoardMoves(string moves) =>
        moves.Split(' ').Where(s => s.Length >= 4).Select(
                m => new BoardMove
                    {
                        From = new BoardPosition
                        {
                            Alphabetical = m[0],
                            Numeral = m[1] - '0'
                        },
                        To = new BoardPosition
                        {
                            Alphabetical = m[2],
                            Numeral = m[3] - '0'
                        }
                    });
};