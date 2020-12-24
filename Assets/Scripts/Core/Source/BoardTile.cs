using UnityEngine;

public class BoardTile : MonoBehaviour, IBoardTile
{
    public BoardPosition Position
    {
        get
        {
            return new BoardPosition
            {
                Alphabetical = this.AlphabeticalUi,
                Numeral = this.NumeralUi
            };
        }
        set
        {
            this.AlphabeticalUi = value.Alphabetical;
            this.NumeralUi = value.Numeral;
        }
    }

    public IChessPiece CurrentPiece { get; set; }

    public GameObject GameObject { get => this.gameObject; }

    public char AlphabeticalUi;

    public int NumeralUi;

    public bool Equals(IBoardTile other) =>
        this.Position.Equals(other.Position);
};