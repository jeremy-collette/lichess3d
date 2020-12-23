using Newtonsoft.Json;
using UnityEngine;

public class Game : MonoBehaviour
{
    private IBoardStateReader boardStateReader;

    private IBoardStateSetter boardStateSetter;

    void Start()
    {
        this.boardStateReader = this.GetComponent<IBoardStateReader>();
        this.boardStateSetter = this.GetComponent<IBoardStateSetter>();
    }

    void Update()
    {
        // Board state from Lichess
        if (!this.boardStateReader.TryGetBoardState(out var boardStateInput))
        {
            return;
        }

        // Update the board using the input from Lichess
        var boardStateOutput = new BoardStateOutput
        {
            Moves = boardStateInput.Moves
        };
        this.boardStateSetter.TrySetBoardState(boardStateOutput);
    }
}
