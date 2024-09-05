using Budget.Application.Repositories;
using Budget.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
namespace Budget.Infrastructure.Repositories;

/// <summary>
/// The UnitOfWork class provides a central point for managing database transactions and
/// saving changes across multiple repositories. It coordinates changes in 
/// <see cref="ICategoryRepository"/> and <see cref="ITransactionRepository"/>.
/// </summary>
/// <remarks>
/// This class follows the Unit of Work design pattern, ensuring that all repository operations 
/// are treated as a single transaction, maintaining data consistency.
/// </remarks>
internal class UnitOfWork : IUnitOfWork
{
    #region Fields

    private readonly BudgetDataContext _dataContext;

    #endregion
    #region Constructors

    public UnitOfWork(BudgetDataContext dataContext, ICategoryRepository categoryRepository, ITransactionRepository transactionRepository)
    {
        _dataContext = dataContext;
        Categories = categoryRepository;
        Transactions = transactionRepository;
    }

    #endregion
    #region Properties

    public ICategoryRepository Categories { get; set; }

    public ITransactionRepository Transactions { get; set; }

    #endregion
    #region Methods

    public async Task<int> SaveAsync()
    {
        try
        {
            return await _dataContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
    }

    #endregion
}
