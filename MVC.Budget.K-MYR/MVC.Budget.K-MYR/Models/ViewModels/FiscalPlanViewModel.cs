namespace MVC.Budget.K_MYR.Models.ViewModels;

public class FiscalPlanViewModel
{
    public FiscalPlanMonthDTO FiscalPlan{ get; set; }
    public IncomeCategory Category { get; set; }
    public Transaction Transaction { get; set; }
    public SearchViewModel Search { get; set; }
    public List<Category> Categories { get; internal set; }
}
