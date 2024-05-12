using DesafioOriginSW_API.Models;

namespace DesafioOriginSW_API.Repository.IRepository
{
    public interface IBankCardRepository : IRepository<BankCard>
    {
        Task<BankCard> Update(BankCard entity);
    }
}
