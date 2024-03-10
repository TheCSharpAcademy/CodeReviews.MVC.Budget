using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcBudget.Models;
using System;
using System.Linq;

namespace MvcBudget.Data;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MvcBudgetContext(
            serviceProvider.GetRequiredService<DbContextOptions<MvcBudgetContext>>()))
        {
            if (context.Transaction.Any() || context.Category.Any())
            {
                return;
            }

            var categories = new Category[]
            {
                new Category { Name = "Food" },
                new Category { Name = "Transportation" },
                new Category { Name = "Shopping" },
                new Category { Name = "Entertainment" },
                new Category { Name = "Bills" }
            };

            context.Category.AddRange(categories);
            context.SaveChanges();

            var transactions = new Transaction[]
            {
                new Transaction
                {
                    Name = "Groceries",
                    Amount = 50.00m,
                    Date = DateTime.Now,
                    CategoryId = categories[0].Id
                },
                new Transaction
                {
                    Name = "Gas",
                    Amount = 30.00m,
                    Date = DateTime.Now,
                    CategoryId = categories[1].Id
                },
                new Transaction
                {
                    Name = "Clothes",
                    Amount = 100.00m,
                    Date = DateTime.Now,
                    CategoryId = categories[2].Id
                },
                new Transaction
                {
                    Name = "Movie tickets",
                    Amount = 25.00m,
                    Date = DateTime.Now,
                    CategoryId = categories[3].Id
                },
                new Transaction
                {
                    Name = "Electricity bill",
                    Amount = 75.00m,
                    Date = DateTime.Now,
                    CategoryId = categories[4].Id
                },
                
                new Transaction
                {
                    Name = "Restaurant",
                    Amount = 60.00m,
                    Date = DateTime.Now,
                    CategoryId = categories[0].Id
                },
                new Transaction
                {
                    Name = "Uber",
                    Amount = 15.00m,
                    Date = DateTime.Now,
                    CategoryId = categories[1].Id
                },
                new Transaction
                {
                    Name = "Gadgets",
                    Amount = 200.00m,
                    Date = DateTime.Now,
                    CategoryId = categories[2].Id
                },
                new Transaction
                {
                    Name = "Concert tickets",
                    Amount = 50.00m,
                    Date = DateTime.Now,
                    CategoryId = categories[3].Id
                },
                new Transaction
                {
                    Name = "Internet bill",
                    Amount = 80.00m,
                    Date = DateTime.Now,
                    CategoryId = categories[4].Id
                },
            };

            context.Transaction.AddRange(transactions);
            context.SaveChanges();
        }
    }
}
