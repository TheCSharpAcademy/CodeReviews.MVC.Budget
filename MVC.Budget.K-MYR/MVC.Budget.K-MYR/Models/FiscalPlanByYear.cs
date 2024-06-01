
namespace MVC.Budget.K_MYR.Models;

public class FiscalPlanByYear
{
    public IEnumerable<CategoryStatistic> CategoryStatistics { get; set; } = new List<CategoryStatistic>();
    public int Id { get; set; }
}
