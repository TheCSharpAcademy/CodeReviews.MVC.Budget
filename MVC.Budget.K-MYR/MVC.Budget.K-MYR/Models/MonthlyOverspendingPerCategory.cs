namespace MVC.Budget.K_MYR.Models;
public class MonthlyOverspendingPerCategory
{
    public string Category { get; set; }
    public IEnumerable<decimal> OverspendingPerMonth { get; set; }
}