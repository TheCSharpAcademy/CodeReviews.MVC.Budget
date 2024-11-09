namespace MVC.Budget.K_MYR.Models;

public class FiscalPlanYearDTO
{

    public decimal HappyEvaluatedTotal { get; set; }
    public decimal UnhappyEvaluatedTotal { get; set; }
    public decimal NecessaryEvaluatedTotal { get; set; }
    public decimal UnnecessaryEvaluatedTotal { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal OverspendingTotal { get; set; }
    public List<decimal> TotalPerMonth { get; set; } = [];
    public List<decimal> HappyPerMonth { get; set; } = [];
    public List<decimal> HappyEvaluatedPerMonth { get; set; } = [];
    public List<decimal> UnhappyPerMonth { get; set; } = [];
    public List<decimal> UnhappyEvaluatedPerMonth { get; set; } = [];
    public List<decimal> NecessaryPerMonth { get; set; } = [];
    public List<decimal> NecessaryEvaluatedPerMonth { get; set; } = [];
    public List<decimal> UnnecessaryPerMonth { get; set; } = [];
    public List<decimal> UnnecessaryEvaluatedPerMonth { get; set; } = [];
    public List<decimal> UnevaluatedPerMonth { get; set; } = [];
    public List<MonthlyOverspendingPerCategory> MonthlyOverspendingPerCategory { get; set; } = [];
    public List<decimal> SavingsPerMonth { get; set; } = [];
    public List<decimal> IncomePerMonth { get; set; } = [];
}