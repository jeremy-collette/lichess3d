using System;
using UnityEngine;

public interface IBoardTile : IEquatable<IBoardTile>
{
    BoardPosition Position { get; set; }

    IChessPiece CurrentPiece { get; set; }

    GameObject GameObject { get; }
};