using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public class CategoryStatisticsRepository : GenericRepository<CategoryStatistic>, ICategoryStatisticsRepository
{
    public CategoryStatisticsRepository(DatabaseContext context) : base(context)
    { }

    public Task<CategoryStatistic?> GetByCategoryIdAndDateTimeAsync(int categoryId, DateTime date)
    {
        return _dbSet.FirstOrDefaultAsync(s => s.CategoryId == categoryId && s.Month.Year == date.Year && s.Month.Month == date.Month);
    }
}
