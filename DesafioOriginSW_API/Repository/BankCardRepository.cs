using DesafioOriginSW_API.Data;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Repository.IRepository;

namespace DesafioOriginSW_API.Repository
{
    public class BankCardRepository : Repository<BankCard>, IBankCardRepository
    {
        private readonly AppDbContext _db;
        public BankCardRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<BankCard> Update(BankCard entity)
        {
            _db.bank_card.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
