using Budget.hasona23.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.hasona23.Data;

public class DataSeed
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        using var context = new BudgetContext(serviceProvider.GetRequiredService<DbContextOptions<BudgetContext>>());
        if (!context.Categories.Any())
        {
            CategoryModel[] categories =
            {
                new CategoryModel { Name = "Income" },
                new CategoryModel { Name = "Expenses" },
                new CategoryModel { Name = "Games and entertainment" },
                new CategoryModel { Name = "Subscriptions" },
                new CategoryModel { Name = "Other" }
            };
            context.Categories.AddRange(categories);
        }

        if (context.Transactions.Any())
        {
            return;
        }

        for (int i = 0; i < 50; i++)
            context.Transactions.Add(new TransactionModel
            {
                Details = $"Buy  {new Random().Next(20)}", Price = new Random().Next(100),
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-new Random().Next(6))
                    .AddMonths(-new Random().Next(11)).AddYears(-new Random().Next(20))),
                Category = context.Categories.ElementAt(new Random().Next(context.Categories.Count())),
            });
        context.SaveChanges();
    }
}