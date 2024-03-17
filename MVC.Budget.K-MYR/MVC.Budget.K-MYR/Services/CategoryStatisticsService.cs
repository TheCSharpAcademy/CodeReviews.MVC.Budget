using MVC.Budget.K_MYR.Data;

namespace MVC.Budget.K_MYR.Services;

public class CategoryStatisticsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryStatisticsService> _logger;
    public CategoryStatisticsService(IUnitOfWork unitOfWork, ILogger<CategoryStatisticsService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task GetYearlyStatistics(int year)
    {
    }
}
