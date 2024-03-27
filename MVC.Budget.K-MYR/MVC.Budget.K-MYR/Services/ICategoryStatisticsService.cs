using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Services
{
    public interface ICategoryStatisticsService
    {
        Task<YearlyStatisticsDto> GetYearlyStatistics(int groupId, int year);
    }
}
