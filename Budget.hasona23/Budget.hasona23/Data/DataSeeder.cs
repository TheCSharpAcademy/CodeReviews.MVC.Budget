using Budget.hasona23.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.hasona23.Data;

public class DataSeeder(IServiceProvider serviceProvider)
{
    public void SeedCategories()
    {
        using (var context = new BudgetDB(serviceProvider.GetService<DbContextOptions<BudgetDB>>()))
        {
            if (context.Category.Any())
                return;
            Console.WriteLine("Seeding categories");
            context.Category.AddRange
            (
                new Category("Subscriptions"),
                new Category("Budgets"),
                new Category("Necessities"),
                new Category("Games and Entertainment")
            );
            context.SaveChanges();
        }
    }

    public void SeedTransactions()
    {
        using (var context = new BudgetDB(serviceProvider.GetService<DbContextOptions<BudgetDB>>()))
        {
            if (context.Transaction.Any() || !context.Category.Any())
                return;
            Console.WriteLine("Seeding Transactions");
            context.AddRange(
                new Transaction("Buy Celeste Game", 19.99f, DateTime.Parse("2021-09-09"),
                    false, context.Category.First(c => c.Name.ToLower().Contains("game"))),
                new Transaction("Buy Hollow Knight Game", 15.99f, DateTime.Parse("2021-09-09"),
                    false, context.Category.First(c => c.Name.ToLower().Contains("game"))),
                new Transaction("Buy Mario Game", 15.99f, DateTime.Parse("2021-09-09"),
                    false, context.Category.First(c => c.Name.ToLower().Contains("game"))),
                new Transaction("Buy Sonic Game", 15.99f, DateTime.Parse("2021-09-09"),
                    false, context.Category.First(c => c.Name.ToLower().Contains("game")))
            );
            context.AddRange(
                new Transaction("Car rent", 19.99f, DateTime.Parse("2021-09-09"),
                    false, context.Category.First(c => c.Name.ToLower().Contains("budget"))),
                new Transaction("Bills", 15.99f, DateTime.Parse("2021-09-09"),
                    false, context.Category.First(c => c.Name.ToLower().Contains("budget"))),
                new Transaction("Car repairs", 15.99f, DateTime.Parse("2021-09-09"),
                    false, context.Category.First(c => c.Name.ToLower().Contains("budget"))),
                new Transaction("Salary", 2000.59f, DateTime.Parse("2021-09-09"),
                    true, context.Category.First(c => c.Name.ToLower().Contains("budget")))
            );
            context.AddRange(
                new Transaction("Premium", 19.99f, DateTime.Parse("2021-09-09"),
                    false, context.Category.First(c => c.Name.ToLower().Contains("subsc"))),
                new Transaction("Chat GPT", 15.99f, DateTime.Parse("2021-09-09"),
                    false, context.Category.First(c => c.Name.ToLower().Contains("subsc"))),
                new Transaction("Food", 15.99f, DateTime.Parse("2021-09-09"),
                    false, context.Category.First(c => c.Name.ToLower().Contains("nec"))),
                new Transaction("Electricity", 15.99f, DateTime.Parse("2021-09-09"),
                    false, context.Category.First(c => c.Name.ToLower().Contains("nec")))
            );
            context.SaveChanges();
        }
    }


    public void SeedData()
    {
        SeedCategories();
        SeedTransactions();
        Console.WriteLine("Data Seeding done");
    }
}