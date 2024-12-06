using Budget.hasona23.Models;
using Microsoft.EntityFrameworkCore;


namespace Budget.hasona23.Data
{
    public class BudgetDB : DbContext
    {
        public BudgetDB (DbContextOptions<BudgetDB> options)
            : base(options)
        {
        }

        public DbSet<Budget.hasona23.Models.Transaction> Transaction { get; set; } = default!;
        public DbSet<Budget.hasona23.Models.Category> Category { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().HasOne(t => t.Category).WithMany(c=>c.Transactions).OnDelete(DeleteBehavior.Cascade);
            
            
        }
    }
}
