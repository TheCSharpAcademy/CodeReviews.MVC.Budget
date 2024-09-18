using System.Linq.Expressions;
using Budget.Application.Repositories;
using Budget.Domain.Entities;
using Budget.Infrastructure.Contexts;
using Budget.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Repositories;

/// <summary>
/// Provides repository operations for managing the Infrastructure layer's Transaction entity.
/// This class implements the <see cref="ITransactionRepository"/> interface, offering 
/// methods to perform CRUD operations against the database using Entity Framework Core.
/// </summary>
internal class TransactionRepository : ITransactionRepository
{
    #region Fields

    private static readonly char[] _separator = [','];
    private readonly BudgetDataContext _dataContext;

    #endregion
    #region Constructors

    public TransactionRepository(BudgetDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    #endregion
    #region Methods

    public async Task CreateAsync(TransactionEntity entity)
    {
        var model = new TransactionModel(entity);
        await _dataContext.Transaction.AddAsync(model);
    }

    public async Task DeleteAsync(Guid id)
    {
        var model = await _dataContext.Transaction.FindAsync(id);
        if (model is not null)
        {
            _dataContext.Transaction.Remove(model);
        }
    }

    public async Task<IEnumerable<TransactionEntity>> ReturnAsync(
        Expression<Func<TransactionEntity, bool>>? filter = null,
        Func<IQueryable<TransactionEntity>, IOrderedQueryable<TransactionEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TransactionModel> query = _dataContext.Transaction;

        // Map the filter expression from the Entity (Domain) to the Model (Infrastructure).
        if (filter is not null)
        {
            var parameter = Expression.Parameter(typeof(TransactionModel), "x");
            var body = Expression.Invoke(filter, Expression.Convert(parameter, typeof(TransactionEntity)));
            var lambda = Expression.Lambda<Func<TransactionModel, bool>>(body, parameter);

            query = query.Where(lambda);
        }

        foreach (var includeProperty in includeProperties.Split(_separator, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var list = await query.ToListAsync();

        var mappedList = list.Select(x => x.MapToDomain()).AsQueryable();

        return orderBy is null
            ? mappedList
            : orderBy(mappedList).ToList();
    }

    public async Task<TransactionEntity?> ReturnAsync(object id)
    {
        var model = await _dataContext.Transaction.FindAsync(id);

        if (model is not null && model.Category is null)
        {
            model.Category = await _dataContext.Category.FindAsync(model.CategoryId);
        }

        return model?.MapToDomain() ?? null;
    }

    public async Task UpdateAsync(TransactionEntity entity)
    {
        var model = await _dataContext.Transaction.FindAsync(entity.Id);
        if (model is not null)
        {
            model.Name = entity.Name ?? "";
            model.Date = entity.Date;
            model.Amount = entity.Amount;
            model.CategoryId = entity.Category!.Id;

            _dataContext.Transaction.Update(model);
        }
    }

    #endregion
}
