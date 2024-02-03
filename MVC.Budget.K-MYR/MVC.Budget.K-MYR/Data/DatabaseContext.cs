using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Data;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>()
            .HasMany(s => s.Categories)
            .WithOne(c => c.Group)
            .HasForeignKey(c => c.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()            
            .HasMany(e => e.Transactions)
            .WithOne(e => e.Category)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .Property(m => m.Budget)
            .HasPrecision(19, 4);


        modelBuilder.Entity<Transaction>()
            .Property(m => m.Amount)
            .HasPrecision(19, 4);
    }
}
