using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Services
{
    public interface ICategoriesService
    {
        Task<List<Category>> GetCategories();
        Category? GetByID(int id);
        ValueTask<Category?> GetByIDAsync(int id);
        Task<List<Category>> GetCategoriesWithUnevaluatedTransactions(int fiscalPlanId, int pageSize = 10);
        Task<T> AddCategory<T>(CategoryPost categoryPost) where T : Category, new();
        Task UpdateCategory(Category category, CategoryPut categoryPut, DateTime month);
        Task DeleteCategory(Category category);
        Task<Category?> GetCategoryWithBudgetLimit(int id, DateTime cutOffDate);
        Task<CategoryMonthDTO?> GetCategoryDataByMonth(int id, DateTime month);
    }
}
