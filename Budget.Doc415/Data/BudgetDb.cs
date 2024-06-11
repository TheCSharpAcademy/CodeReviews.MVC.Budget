using Budget.Doc415.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.Doc415.Data;

public class BudgetDb : DbContext
{
    public BudgetDb(DbContextOptions<BudgetDb> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

}
