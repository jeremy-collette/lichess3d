using Newtonsoft.Json;
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

    private BoardTile[,] chessboard = new BoardTile[8,8];

    private float tileWidth;

    private float tileLength;

    public void Start()
    {
        // Calculate tile dimensions
        var chessboard = GameObject.FindWithTag("Chessboard");
        var renderer = chessboard.GetComponent<Renderer>();
        tileWidth = renderer.bounds.size.x * 0.85f / 8.0f;
        tileLength = renderer.bounds.size.z * 0.85f / 8.0f;

        SetInitialPosition();
    }

    public bool TrySetBoardState(BoardStateOutput boardStateOutput)
    {
        //Debug.Log($"Output board state: {JsonConvert.SerializeObject(boardStateOutput)}");
        SetInitialPosition();

        foreach (var move in boardStateOutput.Moves)
        {
            if (move.Length != 4)
            {
                continue;
            }

            var from = new Position
            {
                Alpha = move[0],
                Num = move[1] - '0'
            };

            var to = new Position
            {
                Alpha = move[2],
                Num = move[3] - '0'
            };
            MovePiece(from, to);
        }
        return true;
    }

    private void MovePiece(Position from, Position to)
    {
        //Debug.Log($"Moving from {JsonConvert.SerializeObject(from)} to {JsonConvert.SerializeObject(to)}...");
        // Empty old virtual space
        var fromX = 7 - (from.Alpha - 'a');
        var fromY = from.Num - 1;
        var movingGameObject = this.chessboard[fromX, fromY].GameObject;
        if (movingGameObject == null)
        {
            Debug.Log("WARNING: GameObject not found at position!");
            return;
        }
        this.chessboard[fromX, fromY].GameObject = null;

        // Fill new virtual space
        var toX = 7 - (to.Alpha - 'a');
        var toY = to.Num - 1;
        var existingPiece =  this.chessboard[toX, toY].GameObject;
        if (existingPiece != null)
        {
            // Hide taken piece
            existingPiece.GetComponent<Renderer>().enabled = false;
        }
        this.chessboard[toX, toY].GameObject = movingGameObject;

        this.SetPiecePosition(movingGameObject, toX, toY);
    }

    private void SetPiecePosition(GameObject gameObject, int tileX, int tileY)
    {
        // Set position of piece
        //Debug.Log($"Setting position for {gameObject.name}...");
        this.GetCoords(tileX, tileY, out var xPos, out var zPos);
        var transform = gameObject.transform;
        transform.position = new Vector3(xPos, transform.position.y, zPos);
        //Debug.Log(transform.position);
    }

    private void GetCoords(int xIndex, int yIndex, out float xPos, out float zPos)
    {
        xPos = (xIndex * tileWidth) - (tileWidth * 3.5f);
        zPos = (-yIndex * tileLength) + (tileLength * 3.75f);
    }

    private void SetInitialPosition()
    {
        // Reset board
        for (var i = 0; i < 8; ++i)
        {
            for (var j = 0; j < 8; ++j)
            {
                chessboard[i,j].GameObject = null;
            }
        }

        // Set initial position White
        chessboard[0,0].GameObject = GameObject.Find("Rook White");
        chessboard[1,0].GameObject = GameObject.Find("Knight White");
        chessboard[2,0].GameObject = GameObject.Find("Bishop White");
        chessboard[3,0].GameObject = GameObject.Find("King White");
        chessboard[4,0].GameObject = GameObject.Find("Queen White");
        chessboard[5,0].GameObject = GameObject.Find("Bishop White 2");
        chessboard[6,0].GameObject = GameObject.Find("Knight White 2");
        chessboard[7,0].GameObject = GameObject.Find("Rook White 2");

        chessboard[0,1].GameObject = GameObject.Find("Pawn White 8");
        chessboard[1,1].GameObject = GameObject.Find("Pawn White 7");
        chessboard[2,1].GameObject = GameObject.Find("Pawn White 6");
        chessboard[3,1].GameObject = GameObject.Find("Pawn White 5");
        chessboard[4,1].GameObject = GameObject.Find("Pawn White 4");
        chessboard[5,1].GameObject = GameObject.Find("Pawn White 3");
        chessboard[6,1].GameObject = GameObject.Find("Pawn White 2");
        chessboard[7,1].GameObject = GameObject.Find("Pawn White");

        // Set initial position Black
        chessboard[0,6].GameObject = GameObject.Find("Pawn Black 8");
        chessboard[1,6].GameObject = GameObject.Find("Pawn Black 7");
        chessboard[2,6].GameObject = GameObject.Find("Pawn Black 6");
        chessboard[3,6].GameObject = GameObject.Find("Pawn Black 5");
        chessboard[4,6].GameObject = GameObject.Find("Pawn Black 4");
        chessboard[5,6].GameObject = GameObject.Find("Pawn Black 3");
        chessboard[6,6].GameObject = GameObject.Find("Pawn Black 2");
        chessboard[7,6].GameObject = GameObject.Find("Pawn Black");

        chessboard[0,7].GameObject = GameObject.Find("Rook Black");
        chessboard[1,7].GameObject = GameObject.Find("Knight Black");
        chessboard[2,7].GameObject = GameObject.Find("Bishop Black");
        chessboard[3,7].GameObject = GameObject.Find("King Black");
        chessboard[4,7].GameObject = GameObject.Find("Queen Black");
        chessboard[5,7].GameObject = GameObject.Find("Bishop Black 2");
        chessboard[6,7].GameObject = GameObject.Find("Knight Black 2");
        chessboard[7,7].GameObject = GameObject.Find("Rook Black 2");

        for (var i = 0; i < 8; ++i)
        {
            for (var j = 0; j < 8; ++j)
            {
                var gameObject = chessboard[i,j].GameObject;
                if (gameObject != null)
                {
                    gameObject.GetComponent<Renderer>().enabled = true;
                    this.SetPiecePosition(chessboard[i,j].GameObject, i, j);
                }
            }
        }

    }
}
