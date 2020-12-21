using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LichessBoardStateReader : MonoBehaviour, IBoardStateReader
{
    private ILichessClient lichessClient;

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

        var moves = new List<string>().AsEnumerable();
        if (message.FullGameUpdateMessage.HasValue)
        {
            moves = message.FullGameUpdateMessage?.State.Moves?.Split(' ');
        }
        else if (message.GameStateMessage.HasValue)
        {
            moves = message.GameStateMessage?.Moves?.Split(' ');
        }

        gameInputUpdate.Moves = moves.ToList();
        return true;
    }
};