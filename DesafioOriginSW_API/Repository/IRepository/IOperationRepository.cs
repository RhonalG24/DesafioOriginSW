using DesafioOriginSW_API.Models;

namespace DesafioOriginSW_API.Repository.IRepository
{
    public interface IOperationRepository : IRepository<Operation>
    {
        Task<Operation> Update(Operation entity);
    }
}
