using DesafioOriginSW_API.Models;

namespace DesafioOriginSW_API.Repository.IRepository
{
    public interface ICardStateRepository : IRepository<CardState>
    {
        Task<CardState> Update(CardState entity);
    }
}
