using System;

[Serializable]
public struct BoardPosition : IEquatable<BoardPosition>
{
    public char Alphabetical { get => this.AlphabeticalUi; set { this.AlphabeticalUi = value; } }

    public int Numeral { get => this.NumeralUi; set { this.NumeralUi = value; } }

    public char AlphabeticalUi;

    public int NumeralUi;

    public bool Equals(BoardPosition other) =>
        this.Alphabetical == other.Alphabetical && this.Numeral == other.Numeral;

    public override string ToString() =>
        $"{this.Alphabetical}{this.Numeral}";
}