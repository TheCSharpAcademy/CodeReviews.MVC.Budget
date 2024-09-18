using System.Linq.Expressions;
using Budget.Application.Repositories;
using Budget.Domain.Entities;
using Budget.Infrastructure.Contexts;
using Budget.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Repositories;

/// <summary>
/// Provides repository operations for managing the Infrastructure layer's Category entity.
/// This class implements the <see cref="ICategoryRepository"/> interface, offering 
/// methods to perform CRUD operations against the database using Entity Framework Core.
/// </summary>
internal class CategoryRepository : ICategoryRepository
{
    #region Fields

    private static readonly char[] _separator = [','];
    private readonly BudgetDataContext _dataContext;

    #endregion
    #region Constructors

    public CategoryRepository(BudgetDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    #endregion
    #region Methods

    public async Task CreateAsync(CategoryEntity entity)
    {
        var model = new CategoryModel(entity);
        await _dataContext.Category.AddAsync(model);
    }

    public async Task DeleteAsync(Guid id)
    {
        var model = await _dataContext.Category.FindAsync(id);
        if (model is not null)
        {
            _dataContext.Category.Remove(model);
        }
    }

    public async Task<IEnumerable<CategoryEntity>> ReturnAsync(
        Expression<Func<CategoryEntity, bool>>? filter = null,
        Func<IQueryable<CategoryEntity>, IOrderedQueryable<CategoryEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<CategoryModel> query = _dataContext.Category;

        // Map the filter expression from the Entity (Domain) to the Model (Infrastructure).
        if (filter is not null)
        {
            var parameter = Expression.Parameter(typeof(CategoryModel), "x");
            var body = Expression.Invoke(filter, Expression.Convert(parameter, typeof(CategoryEntity)));
            var lambda = Expression.Lambda<Func<CategoryModel, bool>>(body, parameter);

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

    public async Task<CategoryEntity?> ReturnAsync(object id)
    {
        var model = await _dataContext.Category.FindAsync(id);
        return model?.MapToDomain() ?? null;
    }

    public async Task UpdateAsync(CategoryEntity entity)
    {
        var model = await _dataContext.Category.FindAsync(entity.Id);
        if (model is not null)
        {
            model.Name = entity.Name ?? "";
            _dataContext.Category.Update(model);
        }
    }

    #endregion
}
