public interface ILichessLoginRetriever
{
    bool TryGetLichessLogin(out LichessLogin login);
}