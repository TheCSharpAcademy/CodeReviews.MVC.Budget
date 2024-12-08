using Budget.hasona23.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.hasona23.Data;

public class BudgetContext : DbContext
{
    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options) { }
    public DbSet<TransactionModel> Transactions { get; set; }
    public DbSet<CategoryModel> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryModel>().HasMany<TransactionModel>().WithOne(x => x.Category);
    }
}