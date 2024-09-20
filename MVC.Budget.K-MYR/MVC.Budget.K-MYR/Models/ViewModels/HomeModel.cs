namespace MVC.Budget.K_MYR.Models.ViewModels;

public class HomeModel
{
    public List<FiscalPlanDTO> FiscalPlans { get; set; } = new List<FiscalPlanDTO>();
    public FiscalPlan FiscalPlan { get; set; }
}
