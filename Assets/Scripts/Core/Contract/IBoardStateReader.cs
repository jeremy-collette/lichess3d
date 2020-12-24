public interface IBoardStateReader
{
    bool TryGetBoardState(out BoardStateInput boardStateInput);
};