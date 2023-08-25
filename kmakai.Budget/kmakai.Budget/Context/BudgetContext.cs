using kmakai.Budget.Models;
using Microsoft.EntityFrameworkCore;

namespace kmakai.Budget.Context;

public class BudgetContext : DbContext
{
    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; } = null!;

    public DbSet<TransactionType> TransactionTypes { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionType>().HasData(
                       new TransactionType
                       {
                           Id = 1,
                           Name = TypeName.Income
                       },
                       new TransactionType
                       {
                           Id = 2,
                           Name = TypeName.Expense
                       });

        modelBuilder.Entity<Category>().HasMany(c => c.Transactions).WithOne(t => t.Category).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>().HasOne(t => t.TransactionType).WithMany(tt => tt.Transactions);

    }
}
