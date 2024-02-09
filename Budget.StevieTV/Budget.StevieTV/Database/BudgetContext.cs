using Budget.StevieTV.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.StevieTV.Database;

public class BudgetContext : DbContext
{
    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
    {
    }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}