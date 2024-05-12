using DesafioOriginSW_API.Data;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Repository.IRepository;

namespace DesafioOriginSW_API.Repository
{
    public class OperationRepository : Repository<Operation>, IOperationRepository
    {
        private readonly AppDbContext _db;
        public OperationRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Operation> Update(Operation entity)
        {
            _db.operation.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
