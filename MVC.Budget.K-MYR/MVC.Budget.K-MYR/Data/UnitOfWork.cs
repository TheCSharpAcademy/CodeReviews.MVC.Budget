using MVC.Budget.K_MYR.Repositories;

namespace MVC.Budget.K_MYR.Data;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly DatabaseContext _context;
    public ICategoriesRepository CategoriesRepository { get; }
    public ITransactionsRepository TransactionsRepository { get; }
    public IGroupsRepository GroupsRepository { get; }
    public ICategoryStatisticsRepository CategoryStatisticsRepository { get; }

    public UnitOfWork(DatabaseContext context, ICategoriesRepository categoriesRepo, ITransactionsRepository transactionsRepo, IGroupsRepository groupsRepo, ICategoryStatisticsRepository categoryStatisticsRepository)
    {
        _context = context;
        CategoriesRepository = categoriesRepo;
        TransactionsRepository = transactionsRepo;
        GroupsRepository = groupsRepo;
        CategoryStatisticsRepository = categoryStatisticsRepository;
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
