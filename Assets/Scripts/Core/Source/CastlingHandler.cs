using System;
using UnityEngine;

public class CastlingHandler : MonoBehaviour, ISpecialMoveHandler
{
    private bool kingMoved = false;

    private bool rookMoved = false;

    public GameObject KingUi;

    public GameObject RookUi;

    public BoardPosition KingDestinationUi;

    public BoardPosition RookDestinationUi;

    public void ProcessMove(IChessboard chessboard, IChessPiece movedPiece, BoardPosition from, BoardPosition to)
    {
        // If this is not the king or rook we are looking for, return
        if (!movedPiece.Equals(this.king) && !movedPiece.Equals(this.rook))
        {
            return;
        }

        // We can't castle if the king or rook has already moved
        if (this.kingMoved || this.rookMoved)
        {
            return;
        }

        // Handle rook
        if (movedPiece.Equals(this.rook))
        {
            Debug.Log($"Castle to {this.KingDestinationUi} is no longer possible as rook has moved.");
            this.rookMoved = true;
            return;
        }

        this.kingMoved = true;

        // If the king is not moving to the castling position, return
        if (!to.Equals(this.KingDestinationUi))
        {
            Debug.Log($"Castle to {this.KingDestinationUi} is no longer possible as king has moved.");
            return;
        }

        // Time to castle! Move the rook.
        Debug.Log($"Castle detected. Moving rook {this.rook.CurrentTile.Position}{this.RookDestinationUi}.");
        chessboard.Move(this.rook.CurrentTile.Position, this.RookDestinationUi);
    }

    public void Reset()
    {
        this.kingMoved = false;
        this.rookMoved = false;
    }

    private IChessPiece king => this.KingUi.GetComponent<IChessPiece>();

    private IChessPiece rook => this.RookUi.GetComponent<IChessPiece>();
}