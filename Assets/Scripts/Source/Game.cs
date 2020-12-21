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
        // Board state is a union of Lichess & player input
        // (but Lichess takes precedence if it exists)
        if (!this.boardStateReader.TryGetBoardState(out var boardStateInput))
        {
            return;
        }
        //Debug.Log($"Input board state: {JsonConvert.SerializeObject(boardStateInput)}");

        // Update the board using both player & Lichess input (if it exists)
        var boardStateOutput = new BoardStateOutput
        {
            Moves = boardStateInput.Moves
        };
        Debug.Log($"Output board state: {JsonConvert.SerializeObject(boardStateOutput)}");
        this.boardStateSetter.TrySetBoardState(boardStateOutput);
    }
}
