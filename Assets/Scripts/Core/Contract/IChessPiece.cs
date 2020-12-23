using UnityEngine;

public interface IChessPiece
{
    PieceType PieceType { get; set; }

    Player Player { get; set; }

    GameObject GameObject { get; }

    IBoardTile InitialTile { get; }

    IBoardTile CurrentTile { get;  set; }
}