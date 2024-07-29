namespace MVC.Budget.K_MYR.Models.ViewModels;

public class FiscalPlanModel
{
    public FiscalPlanMonthDTO FiscalPlan{ get; set; }
    public IncomeCategory Category { get; set; }
    public Transaction Transaction { get; set; }
    public SearchModel Search { get; set; }
}
