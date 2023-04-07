using Budget.Context;
using Budget.Models;

namespace Budget.Repositories;

public class WalletRepository : Repository<Wallet>, IWalletRepository
{
    public WalletRepository(BudgetDbContext budgetDbContext) : base(budgetDbContext)
    {
    }
}