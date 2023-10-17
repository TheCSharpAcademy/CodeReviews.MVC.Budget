namespace MVCBudget.Forser.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserWallet> Wallets { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(t => t.Transactions)
                .WithOne(c => c.Category)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}