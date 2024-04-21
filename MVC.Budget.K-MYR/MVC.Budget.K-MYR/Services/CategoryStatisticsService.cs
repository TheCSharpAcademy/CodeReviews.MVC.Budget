using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Services;

public class CategoryStatisticsService : ICategoryStatisticsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryStatisticsService> _logger;
    public CategoryStatisticsService(IUnitOfWork unitOfWork, ILogger<CategoryStatisticsService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<YearlyStatisticsDto> GetYearlyStatistics(int groupId, int year)
    {
        var monthlyStatistics = await _unitOfWork.CategoriesRepository.GetMonthlyStatistics(groupId, year);

        var grouped = monthlyStatistics.SelectMany(a => a.Statistics).GroupBy(s => s.Month);

        var months = Enumerable.Range(1, 12);

        var stats = new YearlyStatisticsDto
        {   
            TotalPerMonth = months.Select(month => grouped.FirstOrDefault(g => g.Key == month)?.Sum(a => a.TotalSpent) ?? 0),
            HappyPerMonth = months.Select(month => grouped.FirstOrDefault(g => g.Key == month)?.Sum(a => a.HappyTransactions) ?? 0),
            HappyEvaluatedPerMonth = months.Select(month => grouped.FirstOrDefault(g => g.Key == month)?.Sum(a => a.HappyEvaluatedTransactions) ?? 0),
            UnhappyPerMonth = months.Select(month => grouped.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnhappyTransactions) ?? 0),
            UnhappyEvaluatedPerMonth = months.Select(month => grouped.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnhappyEvaluatedTransactions) ?? 0),
            NecessaryPerMonth = months.Select(month => grouped.FirstOrDefault(g => g.Key == month)?.Sum(a => a.NecessaryTransactions) ?? 0),
            NecessaryEvaluatedPerMonth = months.Select(month => grouped.FirstOrDefault(g => g.Key == month)?.Sum(a => a.NecessaryEvaluatedTransactions) ?? 0),
            UnnecessaryPerMonth = months.Select(month => grouped.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnnecessaryTransactions) ?? 0),
            UnnecessaryEvaluatedPerMonth = months.Select(month => grouped.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnnecessaryEvaluatedTransactions) ?? 0),
            UnevaluatedPerMonth = months.Select(month => grouped.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnevaluatedTransactions) ?? 0),
            MonthlyOverspendingPerCategory = monthlyStatistics.Select(c => new MonthlyOverspendingPerCategory
            {
                Category = c.Category,
                OverspendingPerMonth = months.Select(month =>
                    Math.Max(0, (c.Statistics.FirstOrDefault(s => s.Month == month)?.TotalSpent ?? 0) 
                    - (c.BudgetLimits.LastOrDefault(bl => bl.Month.Month <= month)?.Budget ?? c.Budget)))
            }).OrderBy(c => c.Category)
        };

        stats.TotalSpent = stats.TotalPerMonth.Sum();
        stats.OverspendingTotal = stats.MonthlyOverspendingPerCategory.SelectMany(s => s.OverspendingPerMonth).Sum();
        stats.HappyEvaluatedTotal = stats.HappyEvaluatedPerMonth.Sum();        
        stats.UnhappyEvaluatedTotal = stats.TotalSpent - stats.HappyEvaluatedTotal;
        stats.NecessaryEvaluatedTotal = stats.NecessaryEvaluatedPerMonth.Sum();
        stats.UnnecessaryEvaluatedTotal = stats.TotalSpent - stats.NecessaryEvaluatedTotal;

        return stats;
    }
}
