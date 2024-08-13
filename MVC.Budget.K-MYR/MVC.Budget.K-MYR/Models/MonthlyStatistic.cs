namespace MVC.Budget.K_MYR.Models;

public class MonthlyStatistic
{
    public int Month { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal HappyTransactions { get; set; }
    public decimal UnhappyTransactions { get; set; }
    public decimal HappyEvaluatedTransactions { get; set; }
    public decimal UnhappyEvaluatedTransactions { get; set; }
    public decimal NecessaryTransactions { get; set; }
    public decimal UnnecessaryTransactions { get; set; }
    public decimal NecessaryEvaluatedTransactions { get; set; }
    public decimal UnnecessaryEvaluatedTransactions { get; set; }
    public decimal UnevaluatedTransactions { get; set; }
}