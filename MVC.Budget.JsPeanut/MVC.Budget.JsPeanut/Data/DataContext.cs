using Microsoft.EntityFrameworkCore;
using MVC.Budget.JsPeanut.Models;

namespace MVC.Budget.JsPeanut.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Category>()
        //        .HasMany(c => c.Transactions)
        //        .WithOne(t => t.Category)
        //        .HasForeignKey(t => t.CategoryId);

        //    modelBuilder.Entity<Transaction>()
        //        .HasOne(t => t.Category)
        //        .WithMany(t => t.Transactions)
        //        .HasForeignKey(t => t.CategoryId);
        //}
    }
}
