using Microsoft.AspNetCore.Mvc;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Services;

namespace MVC.Budget.K_MYR.API;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly ILogger<CategoriesController> _logger;
    private readonly ICategoryStatisticsService _categoryStatisticsService;

    public GroupsController(ILogger<CategoriesController> logger, ICategoryStatisticsService categoryStatisticsService)
    {
        _logger = logger;
        _categoryStatisticsService = categoryStatisticsService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<YearlyStatisticsDto?>> GetStatisticsByYear([FromRoute]int id, int year)
    {
        var start = DateTime.UtcNow;
        var stats = await _categoryStatisticsService.GetYearlyStatistics(id, year);
        _logger.LogInformation("Statistics Query Duration: {duration} ms", (DateTime.UtcNow - start).TotalMilliseconds);

        return stats;
    }
}