using Budget.Context;

namespace Budget.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BudgetDbContext _dbContext;

    public UnitOfWork(BudgetDbContext dbContext)
    {
        _dbContext = dbContext;
        Categories = new CategoryRepository(dbContext);
        Transactions = new TransactionRepository(dbContext);
        Wallets = new WalletRepository(dbContext);
    }
    
    public ICategoryRepository Categories { get; }
    public ITransactionRepository Transactions { get; }
    public IWalletRepository Wallets { get; }

    public async Task<int> SaveChanges()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}