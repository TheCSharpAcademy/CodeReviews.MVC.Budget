using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.K_MYR.Models;

public class FiscalPlan
{
    public int Id { get; set; }
    public string Name { get; set; }    
    public ICollection<IncomeCategory> IncomeCategories { get; set; }
    public ICollection<ExpenseCategory> ExpenseCategories { get; set; }
}
