using DesafioOriginSW_API.Data;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Repository.IRepository;

namespace DesafioOriginSW_API.Repository
{
    public class CardStateRepository : Repository<CardState>, ICardStateRepository
    {
        private readonly AppDbContext _db;
        public CardStateRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<CardState> Update(CardState entity)
        {
            _db.card_state.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
