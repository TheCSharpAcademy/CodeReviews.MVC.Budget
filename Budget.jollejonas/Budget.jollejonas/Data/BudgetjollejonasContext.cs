using Budget.jollejonas.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.jollejonas.Data
{
    public class BudgetjollejonasContext : DbContext
    {
        public BudgetjollejonasContext(DbContextOptions<BudgetjollejonasContext> options)
            : base(options)
        {
        }

        public DbSet<Budget.jollejonas.Models.Category> Category { get; set; } = default!;
        public DbSet<Budget.jollejonas.Models.Transaction> Transaction { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Transactions)
                .WithOne(t => t.Category)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
