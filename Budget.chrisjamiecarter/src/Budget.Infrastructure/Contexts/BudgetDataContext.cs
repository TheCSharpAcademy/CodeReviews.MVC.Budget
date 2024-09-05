using Budget.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Contexts;

/// <summary>
/// Represents the Entity Framework Core database context for the Budget data store.
/// </summary>
internal class BudgetDataContext : DbContext
{
    #region Constructors

    public BudgetDataContext(DbContextOptions<BudgetDataContext> options) : base(options) { }

    #endregion
    #region Properties

    public DbSet<CategoryModel> Category { get; set; }

    public DbSet<TransactionModel> Transaction { get; set; }

    #endregion
}
