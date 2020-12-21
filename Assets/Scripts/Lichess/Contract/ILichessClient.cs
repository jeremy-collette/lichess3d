using System.Threading.Tasks;

public interface ILichessClient
{
    bool TryGetMessage(out LichessMessage message);
}