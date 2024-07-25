using DesafioOriginSW_API.Models.Entities;

namespace DesafioOriginSW_API.Repository.IRepository
{
    public interface IOperationRepository : IRepository<Operation>
    {
        Task<Operation> Update(Operation entity);
    }
}
