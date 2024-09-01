using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class BudgetDbContext(DbContextOptions<BudgetDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.SeedCategories();
    }
}

public static class ModelBuilderExtensions
{
    public static void SeedCategories(this ModelBuilder builder)
    {
        List<Category> categories = new List<Category>
        {
            new() { Id = 1, Name = "Groceries" },
            new() { Id = 2, Name = "Rent" },
            new() { Id = 3, Name = "Utilities" },
            new() { Id = 4, Name = "Entertainment" },
            new() { Id = 5, Name = "Transportation" },
            new() { Id = 6, Name = "Healthcare" },
            new() { Id = 7, Name = "Dining Out" },
            new() { Id = 8, Name = "Savings" },
            new() { Id = 9, Name = "Insurance" },
            new() { Id = 10, Name = "Education" }
        };
        
        builder.Entity<Category>()
            .HasData(categories);
    }
}