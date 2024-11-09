using MVC.Budget.K_MYR.Models;
using System.Linq.Expressions;

namespace MVC.Budget.K_MYR.Repositories;

public interface ICategoriesRepository : IGenericRepository<Category>
{
    Task<Category?> GetCategoryAsync(int id);
    Task<List<Category>> GetCategoriesWithFilteredTransactionsAsync(Expression<Func<Category, bool>>? filter = null, Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null, Expression<Func<Category, IEnumerable<Transaction>>>? filterTransactions = null);
    Task<Category?> GetCategoryWithBudgetLimits(int id, Expression<Func<Category, IEnumerable<CategoryBudget>>> filter);
    Task<List<CategoryDTO>> GetCategoriesDataByMonth(int fiscalPlanId, DateTime month);
    Task<List<CategoryStatisticsDTO>> GetCategoriesDataByYear(int fiscalPlanId, int year);
    Task<CategoryMonthDTO?> GetCategoryDataByMonth(int id, DateTime month);
}