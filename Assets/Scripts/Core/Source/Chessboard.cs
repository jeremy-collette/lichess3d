using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chessboard : MonoBehaviour, IChessboard
{
    private const int CHESSBOARD_SIZE = 8;

    private IBoardTile[,] chessboard = new IBoardTile[CHESSBOARD_SIZE, CHESSBOARD_SIZE];

    private IChessboardSeeder chessboardSeeder;

    private List<ISpecialMoveHandler> specialMoveHandlers;

    void Start()
    {
        this.chessboardSeeder = this.gameObject.GetComponent<IChessboardSeeder>();
        this.specialMoveHandlers = this.gameObject.GetComponents<ISpecialMoveHandler>().ToList();
        this.SeedChessboard();
    }

    public void Move(BoardPosition from, BoardPosition to)
    {
        Debug.Log($"Processing move {from}{to}...");

        // Transform BoardPosition to chessboard index
        int fromX = from.Alphabetical - 'a';
        int fromY = from.Numeral - 1;
        int toX = to.Alphabetical - 'a';
        int toY = to.Numeral - 1;

        // Pickup piece from old position
        //Debug.Log($"Moving from {fromX},{fromY} to {toX},{toY}...");
        var piece = chessboard[fromX, fromY].CurrentPiece;
        if (piece == null)
        {
            Debug.LogWarning($"Invalid move {from.Alphabetical}{from.Numeral}{to.Alphabetical}{to.Numeral}: could not find piece at {from.Alphabetical}{from.Numeral} ({fromX},{fromY})");
            return;
        }
        chessboard[fromX, fromY].CurrentPiece = null;

        // If new position contains a piece, take it!
        if (chessboard[toX, toY].CurrentPiece != null)
        {
            chessboard[toX, toY].CurrentPiece.GameObject.GetComponent<Renderer>().enabled = false;
        }

        // Move piece to new position
        chessboard[toX, toY].CurrentPiece = piece;
        piece.CurrentTile = chessboard[toX, toY];
        piece.GameObject.transform.position = chessboard[toX, toY].GameObject.transform.position;

        // Handle special moves
        foreach (var specialMoveHandler in this.specialMoveHandlers)
        {
            specialMoveHandler.ProcessMove(this, piece, from, to);
        }
    }

    public void Reset()
    {
        this.SeedChessboard();

        foreach (var specialMoveHandler in this.specialMoveHandlers)
        {
            specialMoveHandler.Reset();
        }
    }

    private void SeedChessboard()
    {
        this.chessboardSeeder.SeedBoard(this.chessboard);
    }
}