using System.Linq.Expressions;
using Budget.Application.Repositories;
using Budget.Domain.Entities;

namespace Budget.Application.Services;

/// <summary>
/// Service class responsible for managing operations related to the Category entity.
/// Provides methods for creating, updating, deleting, and retrieving category data 
/// by interacting with the underlying data repositories through the Unit of Work pattern.
/// </summary>
public class CategoryService : ICategoryService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;

    #endregion
    #region Constructors

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #endregion
    #region Methods

    public async Task CreateAsync(CategoryEntity category)
    {
        await _unitOfWork.Categories.CreateAsync(category);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _unitOfWork.Categories.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<CategoryEntity>> ReturnAsync(
        Expression<Func<CategoryEntity, bool>>? filter = null,
        Func<IQueryable<CategoryEntity>, IOrderedQueryable<CategoryEntity>>? orderBy = null,
        string includeProperties = "")
    {
        return await _unitOfWork.Categories.ReturnAsync(filter, orderBy, includeProperties);
    }

    public async Task<CategoryEntity?> ReturnAsync(Guid id)
    {
        return await _unitOfWork.Categories.ReturnAsync(id);
    }

    public async Task UpdateAsync(CategoryEntity category)
    {
        try
        {
            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception)
        {
            var model = await _unitOfWork.Categories.ReturnAsync(category.Id);

            if (model is not null)
            {
                throw;
            }
        }
    }

    #endregion
}
