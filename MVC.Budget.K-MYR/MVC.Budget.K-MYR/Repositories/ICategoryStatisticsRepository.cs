using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public interface ICategoryStatisticsRepository : IGenericRepository<CategoryStatistic>
{
    Task<CategoryStatistic?> GetByCategoryIdAndDateTimeAsync(int categoryId, DateTime date);
}