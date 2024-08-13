namespace MVC.Budget.K_MYR.Models;

public class FiscalPlanYearDTO
{

    public decimal HappyEvaluatedTotal { get; set; }
    public decimal UnhappyEvaluatedTotal { get; set; }
    public decimal NecessaryEvaluatedTotal { get; set; }
    public decimal UnnecessaryEvaluatedTotal { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal OverspendingTotal { get; set; }
    public IEnumerable<decimal> TotalPerMonth { get; set; } = Enumerable.Empty<decimal>();
    public IEnumerable<decimal> HappyPerMonth { get; set; } = Enumerable.Empty<decimal>();
    public IEnumerable<decimal> HappyEvaluatedPerMonth { get; set; } = Enumerable.Empty<decimal>();
    public IEnumerable<decimal> UnhappyPerMonth { get; set; } = Enumerable.Empty<decimal>();
    public IEnumerable<decimal> UnhappyEvaluatedPerMonth { get; set; } = Enumerable.Empty<decimal>();
    public IEnumerable<decimal> NecessaryPerMonth { get; set; } = Enumerable.Empty<decimal>();
    public IEnumerable<decimal> NecessaryEvaluatedPerMonth { get; set; } = Enumerable.Empty<decimal>();
    public IEnumerable<decimal> UnnecessaryPerMonth { get; set; } = Enumerable.Empty<decimal>();
    public IEnumerable<decimal> UnnecessaryEvaluatedPerMonth { get; set; } = Enumerable.Empty<decimal>();
    public IEnumerable<decimal> UnevaluatedPerMonth { get; set; } = Enumerable.Empty<decimal>();
    public IEnumerable<MonthlyOverspendingPerCategory> MonthlyOverspendingPerCategory { get; set; } = Enumerable.Empty<MonthlyOverspendingPerCategory>();
    public IEnumerable<decimal> SavingsPerMonth { get; set; } = Enumerable.Empty<decimal>();
    public IEnumerable<decimal> IncomePerMonth { get; set; } = Enumerable.Empty<decimal>();
}