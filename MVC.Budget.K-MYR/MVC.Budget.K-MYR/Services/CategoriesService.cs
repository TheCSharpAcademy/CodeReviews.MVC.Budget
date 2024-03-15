using MVC.Budget.K_MYR.API;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;
using System.Linq.Expressions;

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
        return _unitOfWork.CategoriesRepository.GetByIdAsync(id);
    }

    public Category? GetByID(int id)
    {
        return _unitOfWork.CategoriesRepository.GetByID(id);
    }

    public Task<List<Category>> GetCategoriesWithUnevaluatedTransactions()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-14);

        return _unitOfWork.CategoriesRepository.GetCategoriesWithFilteredTransactionsAsync(
                c => c.GroupId == 2,
                q => q.OrderBy(c => c.Name),
                c => c.Transactions.Where(t => t.Evaluated == false && t.DateTime < cutoffDate)
                    .OrderByDescending(d => d.DateTime));
    }

    public async Task<Category> AddCategory(CategoryPost categoryPost)
    {
        var category = new Category()
        {
            Name = categoryPost.Name,
            Budget = categoryPost.Budget,
            GroupId = categoryPost.GroupId,
        };

        _unitOfWork.CategoriesRepository.Insert(category);

        await _unitOfWork.Save();

        var categoryStatistics = new CategoryStatistic()
        {
            CategoryId = category.Id,
            Budget = category.Budget,
            Month = DateTime.UtcNow
        };

        _unitOfWork.CategoryStatisticsRepository.Insert(categoryStatistics);
        
        await _unitOfWork.Save();

        return category;
    }

    public async Task UpdateCategory(Category category, CategoryPut categoryPut)
    {
        category.Name = categoryPut.Name;
        category.Budget = categoryPut.Budget;
        category.GroupId = categoryPut.GroupId;

        var currentDate = DateTime.UtcNow;

        var statistic = category.Statistics.Where(s => s.Month.Year == currentDate.Year && s.Month.Month == currentDate.Month)
                                           .SingleOrDefault();

        if(statistic is null)
        {
            _unitOfWork.CategoryStatisticsRepository.Insert(new CategoryStatistic
            {
                CategoryId = category.Id,
                Budget = category.Budget,
                Month = DateTime.UtcNow
            });
        }
        else
        {
            statistic.Budget = category.Budget;
            statistic.Overspending = Math.Max(0, statistic.TotalSpent - statistic.Budget);

            _unitOfWork.CategoryStatisticsRepository.Update(statistic);            
        }

        _unitOfWork.CategoriesRepository.Update(category);
        await _unitOfWork.Save();
    }

    public async Task DeleteCategory(Category category)
    {
        _unitOfWork.CategoriesRepository.Delete(category);
        await _unitOfWork.Save();
    }

    public Task<Category?> GetCategoryWithFilteredStatistics(int id, Expression<Func<CategoryStatistic, bool>> filter)
    {
        return _unitOfWork.CategoriesRepository.GetCategoryWithFilteredStatistics(id, filter);
    }
}
