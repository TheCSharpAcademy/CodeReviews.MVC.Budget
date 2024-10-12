namespace MVC.Budget.K_MYR.Models.ViewModels;

public class IndexViewModel
{
    public List<FiscalPlanDTO> FiscalPlans { get; set; } = [];
    public FiscalPlan FiscalPlan { get; set; } = new();
}
