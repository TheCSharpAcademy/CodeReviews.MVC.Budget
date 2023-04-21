using Budget.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.Context;

public class BudgetDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }

    public BudgetDbContext(DbContextOptions<BudgetDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>()
            .HasData(new Wallet
            {
                Id = 1,
                Name = "Main Wallet",
                Income = 5000.00M,
                Expenses = 800M,
                Balance = 4200M
            },
            new Wallet
            {
                Id = 2,
                Name = "Test Wallet",
                Income = 5000.00M,
                Expenses = 300M,
                Balance = 4700.00M
            });

        modelBuilder.Entity<Category>()
            .HasData(new Category
            {
                Id = 1,
                Name = "Housing"
            },
            new Category
            {
                Id = 2,
                Name = "Transportation"
            },
            new Category
            {
                Id = 3,
                Name = "Food"
            },
            new Category
            {
                Id = 4,
                Name = "Utilities"
            },
            new Category
            {
                Id = 5,
                Name = "Insurance"
            },
            new Category
            {
                Id = 6,
                Name = "Medical & Healthcare"
            },
            new Category
            {
                Id = 7,
                Name = "Saving, Investing & Dept Payments"
            },
            new Category
            {
                Id = 8,
                Name = "Personal Spending"
            },
            new Category
            {
                Id = 9,
                Name = "Recreation & Entertainment"
            },
            new Category
            {
                Id = 10,
                Name = "Miscellaneous"
            });

        modelBuilder.Entity<Transaction>()
            .HasData(new Transaction
            {
                Id = 1,
                Name = "Computer Accessories",
                Description = "I bought a new laptop, external keyboard and mouse",
                Date = DateTime.Now,
                Cost = 500.00M,
                Month = Month.March,
                WalletId = 1,
                CategoryId = 4
            },
            new Transaction
            {
                Id = 2,
                Name = "Weekly fruit stocking",
                Description = "I bought a bunch of bananas, grapes and 7 oranges",
                Date = DateTime.Now,
                Cost = 150.00M,
                Month = Month.March,
                WalletId = 1,
                CategoryId = 3
            },
            new Transaction
            {
                Id = 3,
                Name = "Trip to Belgorod",
                Description = "Went to assist Dominion in her cake business",
                Date = DateTime.Now,
                Cost = 150.00M,
                Month = Month.March,
                WalletId = 1,
                CategoryId = 2
            },
            new Transaction
            {
                Id = 4,
                Name = "Annual Health Insurance",
                Description = "Paid Annual Health Insurance",
                Date = DateTime.Now,
                Cost = 300.00M,
                Month = Month.March,
                WalletId = 2,
                CategoryId = 5
            });
    }
}