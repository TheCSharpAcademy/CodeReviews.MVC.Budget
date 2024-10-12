using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Services;

public class CategoriesService : ICategoriesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoriesService> _logger;

    public CategoriesService(IUnitOfWork unitOfWork, ILogger<CategoriesService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public Task<List<Category>> GetCategories()
    {
        return _unitOfWork.CategoriesRepository.GetAsync();
    }

    public ValueTask<Category?> GetByIDAsync(int id)
    {
        return _unitOfWork.CategoriesRepository.GetByIDAsync(id);
    }

    public Category? GetByID(int id)
    {
        return _unitOfWork.CategoriesRepository.GetByID(id);
    }

    public Task<Category?> GetCategoryWithBudgetLimit(int id, DateTime cutOffDate)
    {       
        return _unitOfWork.CategoriesRepository.GetCategoryWithBudgetLimits(id, c => c.PreviousBudgets.Where(s => cutOffDate <= s.Month));
    }

    public async Task<CategoryMonthDTO?> GetCategoryDataByMonth(int id, DateTime month)
    {
        var categoryDTO = await _unitOfWork.CategoriesRepository.GetCategoryDataByMonth(id, month);
        if (categoryDTO != null)
        {
            categoryDTO.Month = month;
        }
        return categoryDTO;
    }

    public Task<List<Category>> GetCategoriesWithUnevaluatedTransactions(int fiscalPlanId, int pageSize = 10)
    {
        var date = DateTime.UtcNow.AddDays(-14);
        var cutOffDate = new DateTime(date.Year, date.Month, date.Day);

        return _unitOfWork.CategoriesRepository.GetCategoriesWithFilteredTransactionsAsync(
                c => c.FiscalPlanId == fiscalPlanId,
                q => q.OrderBy(c => c.Name),
                c => c.Transactions.Where(t => t.IsEvaluated == false && t.DateTime < cutOffDate)
                                   .OrderBy(d => d.DateTime)
                                   .Take(pageSize));
    }

    public async Task<T> AddCategory<T>(CategoryPost categoryPost) where T : Category, new()
    {
        var category = new T()
        {
            Name = categoryPost.Name,
            Budget = categoryPost.Budget,
            FiscalPlanId = categoryPost.FiscalPlanId,
        };

        var categoryStatistics = new CategoryBudget()
        {
            CategoryId = category.Id,
            Budget = category.Budget,
            Month = DateTime.UtcNow
        };

        category.PreviousBudgets.Add(categoryStatistics);

        _unitOfWork.CategoriesRepository.Insert(category);

        await _unitOfWork.Save();

        return category;
    }

    public async Task UpdateCategory(Category category, CategoryPut categoryPut, DateTime month)
    {
        if (categoryPut.Budget != category.Budget) 
        {
            var currentBudget = category.PreviousBudgets.SingleOrDefault(b => b.Month.Year == month.Year && b.Month.Month == month.Month);

            if (currentBudget is null)
            {
                _unitOfWork.CategoryBudgetsRepository.Insert(new CategoryBudget
                {
                    CategoryId = category.Id,
                    Budget = categoryPut.Budget,
                    Month = month
                });
            }
            else
            {
                currentBudget.Budget = categoryPut.Budget;
            }

            if ((DateTime.UtcNow.Month == month.Month && DateTime.UtcNow.Year == month.Year) || 
                !category.PreviousBudgets.Any(b => b.Month.Month != month.Month || b.Month.Year != month.Year))
            {
                category.Budget = categoryPut.Budget;
            }
        }

        category.Name = categoryPut.Name;

        await _unitOfWork.Save();
    }

    public async Task DeleteCategory(Category category)
    {
        _unitOfWork.CategoriesRepository.Delete(category);
        await _unitOfWork.Save();
    }    
}
