using MVC.Budget.K_MYR.Models;
using System.Linq.Expressions;

namespace MVC.Budget.K_MYR.Services
{
    public interface ICategoriesService
    {
        Task<Category> AddCategory(CategoryPost categoryPost);
        Task<List<Category>> GetCategories();
        ValueTask<Category?> GetByIDAsync(int id);
        Task UpdateCategory(Category category, CategoryPut categoryPut);
        Task<List<Category>> GetCategoriesWithUnevaluatedTransactions();
        Category? GetByID(int id);
        Task DeleteCategory(Category category);
        Task<Category?> GetCategoryWithFilteredStatistics(int id, Expression<Func<CategoryStatistic, bool>> filter);
    }
}