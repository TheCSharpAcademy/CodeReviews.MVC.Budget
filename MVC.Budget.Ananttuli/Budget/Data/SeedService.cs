using Bogus;
using Budget.CategoriesModule.Models;
using Budget.TransactionsModule.Models;

namespace Budget.Data;

public static class SeedService
{
    public static void Seed(BudgetDb db)
    {
        var numCategories = db.Categories.Count();

        if (numCategories > 0) return;

        var categories = new List<string>(["Shopping", "Groceries", "Eating out", "Entertainment", "Charity"])
            .Select(name => new Category { Name = name })
            .ToList();

        db.Categories.AddRange(categories);
        db.SaveChanges();

        var places = new[]
        {
            "Starbucks", "Walmart", "Amazon", "Apple Store", "McDonald's",
            "Home Depot", "Target", "Best Buy", "Subway", "Gas Station",
            "Cinema", "Gym", "Charity Event", "Bookstore", "Supermarket"
        };

        var faker = new Faker<Transaction>()
            .RuleFor(t => t.Date, f => f.Date.Past(5))
            .RuleFor(t => t.Description, f => f.PickRandom(places))
            .RuleFor(t => t.Amount, f => decimal.Round(f.Finance.Amount(1), 2))
            .RuleFor(t => t.CategoryId, f => f.PickRandom(categories.Select(c => c.Id).ToArray()));

        var transactions = faker.Generate(1000);

        db.Transactions.AddRange(transactions);
        db.SaveChanges();
    }
}