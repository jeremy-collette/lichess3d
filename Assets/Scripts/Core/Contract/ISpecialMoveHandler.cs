public interface ISpecialMoveHandler
{
    void ProcessMove(IChessboard chessboard, IChessPiece movedPiece, BoardPosition from, BoardPosition to);

    void Reset();
}
