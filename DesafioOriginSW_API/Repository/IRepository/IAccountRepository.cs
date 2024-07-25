using DesafioOriginSW_API.Models.Entities;

namespace DesafioOriginSW_API.Repository.IRepository
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> Update(Account entity);
    }
}
