using Budget.StevieTV.Enums;
using Budget.StevieTV.Models;

namespace Budget.StevieTV.Database;

public class SeedDatabase
{
    public static void Seed(BudgetContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category
                {
                    Name = "Rent"
                },
                new Category
                {
                    Name = "Wages"
                },
                new Category
                {
                    Name = "Groceries"
                },
                new Category
                {
                    Name = "Video Games"
                },
                new Category
                {
                    Name = "Subscriptions"
                }
            );

            context.SaveChangesAsync();
        }

        var rentCategory = context.Categories.FirstOrDefault(c => c.Name == "Rent")!.Id;
        var wagesCategory = context.Categories.FirstOrDefault(c => c.Name == "Wages")!.Id;
        var groceriesCategory = context.Categories.FirstOrDefault(c => c.Name == "Groceries")!.Id;
        var videoGamesCategory = context.Categories.FirstOrDefault(c => c.Name == "Video Games")!.Id;
        var subscriptionsCategory = context.Categories.FirstOrDefault(c => c.Name == "Subscriptions")!.Id;

        if (!context.Transactions.Any())
        {
            context.Transactions.AddRange(
                new Transaction
                {
                    TransactionType = TransactionType.Income,
                    Date = DateTime.Parse("01/01/2024"),
                    Description = "Salary Jan 2024",
                    Amount = 2250,
                    CategoryId = wagesCategory
                },
                new Transaction
                {
                    TransactionType = TransactionType.Income,
                    Date = DateTime.Parse("01/02/2024"),
                    Description = "Salary Feb 2024",
                    Amount = 2250,
                    CategoryId = wagesCategory
                },
                new Transaction
                {
                    TransactionType = TransactionType.Income,
                    Date = DateTime.Parse("01/03/2024"),
                    Description = "Salary Mar 2024",
                    Amount = 2250,
                    CategoryId = wagesCategory
                },
                new Transaction
                {
                    TransactionType = TransactionType.Expense,
                    Date = DateTime.Parse("25/01/2024"),
                    Description = "Rent Jan 2024",
                    Amount = 875.25M,
                    CategoryId = rentCategory
                },
                new Transaction
                {
                    TransactionType = TransactionType.Expense,
                    Date = DateTime.Parse("25/02/2024"),
                    Description = "Rent Feb 2024",
                    Amount = 875.25M,
                    CategoryId = rentCategory
                },
                new Transaction
                {
                    TransactionType = TransactionType.Expense,
                    Date = DateTime.Parse("25/02/2024"),
                    Description = "Spotify Year Membership",
                    Amount = 129.36M,
                    CategoryId = subscriptionsCategory
                }
            );

            context.SaveChangesAsync();
        }
    }
}