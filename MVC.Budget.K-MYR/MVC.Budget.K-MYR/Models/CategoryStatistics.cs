namespace MVC.Budget.K_MYR.Models;

public class CategoryStatistics
{
    public string Category { get; set; }
    public IEnumerable<MonthlyStatistics> Statistics { get; set; }
    public IEnumerable<BudgetLimit> BudgetLimits{ get; set; }
    public decimal Budget { get; internal set; }
}