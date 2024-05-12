using DesafioOriginSW_API.Data;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Repository.IRepository;

namespace DesafioOriginSW_API.Repository
{
    public class OperationTypeRepository : Repository<OperationType>, IOperationTypeRepository
    {
        private readonly AppDbContext _db;
        public OperationTypeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<OperationType> Update(OperationType entity)
        {
            _db.operation_type.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
