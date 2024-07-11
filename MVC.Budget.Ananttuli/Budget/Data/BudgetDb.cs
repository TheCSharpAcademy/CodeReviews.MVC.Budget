using Budget.CategoriesModule.Models;
using Budget.TransactionsModule.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.Data;

public class BudgetDb : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }

    public BudgetDb(DbContextOptions options) : base(options)
    { }
}