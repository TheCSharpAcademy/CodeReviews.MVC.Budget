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
        //    modelBuilder.Entity<Category>().HasData(
        //        new Category
        //        {
        //            Id = 0,
        //            ImageUrl = "https://img.icons8.com/fluency/48/maintenance--v1.pn",
        //            Name = "Food",
        //            TotalValue = 0
        //        },
        //        new Category
        //        {
        //            Id = 1,
        //            ImageUrl = "https://img.icons8.com/bubbles/50/bus.png",
        //            Name = "Transportation",
        //            TotalValue = 0
        //        },
        //        new Category
        //        {
        //            Id = 2,
        //            ImageUrl = "https://img.icons8.com/bubbles/50/cottage.png",
        //            Name = "Housing",
        //            TotalValue = 0
        //        },
        //        new Category
        //        {
        //            Id = 3,
        //            ImageUrl = "https://img.icons8.com/bubbles/50/maintenance.pn",
        //            Name = "Utilities",
        //            TotalValue = 0
        //        },
        //        new Category
        //        {
        //            Id = 4,
        //            ImageUrl = "https://img.icons8.com/bubbles/50/netflix.png",
        //            Name = "Subscriptions",
        //            TotalValue = 0
        //        },
        //        new Category
        //        {
        //            Id = 5,
        //            ImageUrl = "https://img.icons8.com/clouds/100/hospital.pn",
        //            Name = "Healthcare",
        //            TotalValue = 0
        //        },
        //        new Category
        //        {
        //            Id = 6,
        //            ImageUrl = "https://img.icons8.com/clouds/100/bank-card-back-side.png",
        //            Name = "Personal expenses",
        //            TotalValue = 0
        //        },
        //        new Category
        //        {
        //            Id = 7,
        //            ImageUrl = "https://img.icons8.com/arcade/64/kawaii-pizza.png",
        //            Name = "Savings and investments",
        //            TotalValue = 0
        //        },
        //        new Category
        //        {
        //            Id = 8,
        //            ImageUrl = "https://img.icons8.com/arcade/64/kawaii-pizza.png",
        //            Name = "Debt payment",
        //            TotalValue = 0
        //        },
        //        new Category
        //        {
        //            Id = 9,
        //            ImageUrl = "https://img.icons8.com/arcade/64/kawaii-pizza.png",
        //            Name = "Miscellaneous expenses",
        //            TotalValue = 0
        //        });
        //}
    }
}
