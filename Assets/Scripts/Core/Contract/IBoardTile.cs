using UnityEngine;

public interface IBoardTile
{
    BoardPosition Position { get; set; }

    IChessPiece CurrentPiece { get; set; }

    GameObject GameObject { get; }
};