using DesafioOriginSW_API.Models;

namespace DesafioOriginSW_API.Handlers.IHandler
{
    public interface IAccountHandler
    {
        Task<IEnumerable<Account>> GetAllAccounts();
    }
}
