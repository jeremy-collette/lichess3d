public interface IChessboard
{
    void Move(BoardPosition from, BoardPosition to);

    void Reset();
};