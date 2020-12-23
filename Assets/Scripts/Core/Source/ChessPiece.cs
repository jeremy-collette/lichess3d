using UnityEngine;

public class ChessPiece : MonoBehaviour, IChessPiece
{
    public PieceType PieceType { get => PieceTypeUi;  set { PieceTypeUi = value; } }

    public Player Player { get => PlayerUi;  set { PlayerUi = value; } }

    public GameObject GameObject { get => this.gameObject; }

    public IBoardTile InitialTile { get => this.InitialTileUi.gameObject.GetComponent<IBoardTile>(); }

    public IBoardTile CurrentTile { get; set; }

    public Player PlayerUi;

    public PieceType PieceTypeUi;

    public GameObject InitialTileUi;
}