using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Data;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<FiscalPlan> FiscalPlans { get; set; }
    public DbSet<IncomeCategory> IncomeCategories { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<CategoryBudget> CategoryBudgets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FiscalPlan>()
            .HasMany(f => f.IncomeCategories)
            .WithOne(c => c.FiscalPlan)
            .HasForeignKey(c => c.FiscalPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FiscalPlan>()
            .HasMany(f => f.ExpenseCategories)
            .WithOne(c => c.FiscalPlan)
            .HasForeignKey(c => c.FiscalPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
           .HasDiscriminator<int>("CategoryType")
           .HasValue<IncomeCategory>(1)
           .HasValue<ExpenseCategory>(2);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Transactions)
            .WithOne(t => t.Category)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.PreviousBudgets)
            .WithOne(s => s.Category)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .Property(m => m.Budget)
            .HasPrecision(19, 4);

        modelBuilder.Entity<CategoryBudget>()
            .Property(m => m.Budget)
            .HasPrecision(19, 4);

        modelBuilder.Entity<Transaction>()
            .Property(m => m.Amount)
            .HasPrecision(19, 4);

        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.DateTime);
    }
}
