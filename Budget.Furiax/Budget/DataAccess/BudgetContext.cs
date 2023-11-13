using Microsoft.EntityFrameworkCore;
using Budget.Models;

namespace Budget.DataAccess
{
    public class BudgetContext : DbContext
    {
        public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
        {
        }

        public DbSet<TransactionModel> Transactions { get; set; } = null!;
        public DbSet<CategoryModel> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryModel>()
                .HasMany(c => c.Transactions)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId);
            modelBuilder.Entity<TransactionModel>()
                .HasOne(t => t.Category);

        }
    }
}
