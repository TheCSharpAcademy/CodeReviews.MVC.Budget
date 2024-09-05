using Bogus;
using Budget.Infrastructure.Contexts;
using Budget.Infrastructure.Models;

namespace Budget.Infrastructure.Services;

/// <summary>
/// Provides methods to seed the database with initial data.
/// This service adds a defined set of default Category records
/// and then uses Bogus to add fake Transaction records.
/// </summary>
internal class SeederService : ISeederService
{
    #region Fields

    private readonly string[] _categories =
    [
        "Bills",
        "Charity",
        "Eating Out",
        "Entertainment",
        "Expenses",
        "Family",
        "Finances",
        "General",
        "Gifts",
        "Groceries",
        "Holidays",
        "Personal Care",
        "Savings",
        "Shopping",
        "Transfers",
        "Transport"
    ];

    private readonly BudgetDataContext _context;

    #endregion
    #region Constructors

    public SeederService(BudgetDataContext context)
    {
        _context = context;
    }

    #endregion
    #region Methods

    public void SeedDatabase()
    {
        // Categories first.
        SeedCategories();

        // Transactions require Categories.
        SeedTransactions();
    }

    private void SeedCategories()
    {
        if (_context.Category.Any())
        {
            return;
        }

        foreach (var category in _categories)
        {
            _context.Category.Add(new CategoryModel { Name = category });

        }
        _context.SaveChanges();
    }

    private void SeedTransactions()
    {
        Randomizer.Seed = new Random(19890309);

        if (_context.Transaction.Any())
        {
            return;
        }

        var categories = _context.Category.ToList();

        var fakeTransactions = new Faker<TransactionModel>()
            .RuleFor(t => t.CategoryId, f => f.PickRandom(categories).Id)
            .RuleFor(t => t.Date, f => f.Date.Past(1))
            .RuleFor(t => t.Name, (f, t) => f.Commerce.Product())
            .RuleFor(t => t.Amount, f => f.Finance.Amount(0.01M));

        foreach (var transaction in fakeTransactions.Generate(100))
        {
            _context.Transaction.Add(transaction);
        }

        _context.SaveChanges();
    }

    #endregion
}
