using UnityEngine;

public class ChessboardSeeder : MonoBehaviour, IChessboardSeeder
{
    public void SeedBoard(IBoardTile[,] chessboard)
    {
        this.SetupBoardTiles(chessboard);
        this.ResetPiecePositions(chessboard);
    }

    private void SetupBoardTiles(IBoardTile[,] chessboard)
    {
        for (var y = 0; y < 8; ++y)
        {
            var row = y+1;
            var rowGameObject = GameObject.Find($"Row {row}");
            for (var x = 0; x < 8; ++x)
            {
                var alpha = (char)('a' + x);
                var tileGameObject = rowGameObject.transform.Find($"{alpha}").gameObject;

                var tileComponent = tileGameObject.GetComponent<BoardTile>() ?? tileGameObject.AddComponent<BoardTile>();
                tileComponent.AlphabeticalUi = alpha;
                tileComponent.NumeralUi = row;
                tileComponent.CurrentPiece = null;
                chessboard[x, y] = tileComponent;
            }
        }
    }

    private void ResetPiecePositions(IBoardTile[,] chessboard)
    {
        foreach (var chessPieceGameObject in GameObject.FindGameObjectsWithTag("ChessPiece"))
        {
            //Debug.Log($"Resetting {chessPieceGameObject.name}");
            var chessPieceComponent = chessPieceGameObject.GetComponent<IChessPiece>();
            var initialTile = chessPieceComponent.InitialTile;
            chessPieceComponent.CurrentTile = initialTile;
            initialTile.CurrentPiece = chessPieceComponent;
            chessPieceGameObject.transform.position = initialTile.GameObject.transform.position;
            chessPieceGameObject.GetComponent<Renderer>().enabled = true;
        }
    }

}
