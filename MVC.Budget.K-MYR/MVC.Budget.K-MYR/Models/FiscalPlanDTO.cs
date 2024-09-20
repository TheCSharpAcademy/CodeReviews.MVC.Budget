namespace MVC.Budget.K_MYR.Models;

public class FiscalPlanDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal BudgetIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal BudgetExpenses { get; set; }
}
