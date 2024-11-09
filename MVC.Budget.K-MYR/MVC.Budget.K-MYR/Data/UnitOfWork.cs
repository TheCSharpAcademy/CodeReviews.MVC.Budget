using MVC.Budget.K_MYR.Repositories;

namespace MVC.Budget.K_MYR.Data;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly DatabaseContext _context;
    public ICategoriesRepository CategoriesRepository { get; }
    public ITransactionsRepository TransactionsRepository { get; }
    public ICategoryBudgetsRepository CategoryBudgetsRepository { get; }
    public IFiscalPlansRepository FiscalPlansRepository { get; }

    public UnitOfWork(DatabaseContext context, ICategoriesRepository categoriesRepository, ITransactionsRepository transactionsRepository, ICategoryBudgetsRepository categoryBudgetsRepository, IFiscalPlansRepository fiscalPlansRepository)
    {
        _context = context;
        CategoriesRepository = categoriesRepository;
        TransactionsRepository = transactionsRepository;
        CategoryBudgetsRepository = categoryBudgetsRepository;
        FiscalPlansRepository = fiscalPlansRepository;
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
