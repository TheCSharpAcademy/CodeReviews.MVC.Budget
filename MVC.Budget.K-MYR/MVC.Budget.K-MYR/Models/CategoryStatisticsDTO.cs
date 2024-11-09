namespace MVC.Budget.K_MYR.Models;

public class CategoryStatisticsDTO
{
    public string Category { get; set; }
    public IEnumerable<MonthlyStatistic> Statistics { get; set; } = [];
    public IEnumerable<BudgetLimit> BudgetLimits { get; set; } = [];
    public decimal Budget { get; internal set; }
    public int CategoryType { get; internal set; }
}