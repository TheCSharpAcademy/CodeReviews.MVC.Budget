namespace MVC.Budget.K_MYR.Models;

public class CategoryMonthDTO
{
    public DateTime Month { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }
    public decimal Budget { get; set; }
    public decimal Total { get; set; }
    public decimal NecessaryTotal { get; set; }
    public decimal HappyTotal { get; set; }
    public int CategoryType { get; set; }
    public BudgetLimit? BudgetLimit { get; set; }
    public int FiscalPlanId { get; set; }
}
