using DesafioOriginSW_API.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioOriginSW_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<OperationType> operation_type { get; set; }
        public DbSet<Operation> operation { get; set; }
        public DbSet<CardState> card_state { get; set; }
        public DbSet<BankCard> bank_card { get; set; }
        public DbSet<Account> account { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OperationType>().HasData(
                new OperationType()
                {
                    id_operation_type = 1,
                    name = "balance"
                },
                new OperationType()
                {
                    id_operation_type = 2,
                    name = "retiro"
                }
            );
        }
    }
}
