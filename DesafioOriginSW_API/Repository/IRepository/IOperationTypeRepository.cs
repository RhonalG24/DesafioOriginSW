using DesafioOriginSW_API.Models.Entities;

namespace DesafioOriginSW_API.Repository.IRepository
{
    public interface IOperationTypeRepository : IRepository<OperationType>
    {
        Task<OperationType> Update(OperationType entity);
    }
}
