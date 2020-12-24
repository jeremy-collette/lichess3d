using UnityEngine;

public class LocalBoardStateSetter : MonoBehaviour, IBoardStateSetter
{
    private struct BoardTile
    {
        public GameObject GameObject { get; set; }
    };

    private struct Position
    {
        public char Alpha;

        public int Num;
    };

    private IChessboard chessboard;

    public void Start()
    {
        this.chessboard = GameObject.Find("Chessboard").GetComponent<IChessboard>();
    }

    public bool TrySetBoardState(BoardStateOutput boardStateOutput)
    {
        //Debug.Log($"Output board state: {JsonConvert.SerializeObject(boardStateOutput)}");
        this.chessboard.Reset();

        foreach (var move in boardStateOutput.Moves)
        {
            this.chessboard.Move(move.From, move.To);
        }

        return true;
    }
}
