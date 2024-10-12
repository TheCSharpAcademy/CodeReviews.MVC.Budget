using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.K_MYR.Models;

public class FiscalPlan
{
    public int Id { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 0, ErrorMessage = "'Name' must be between 1 and 50 characters.")]
    public string Name { get; set; } = string.Empty; 
    public ICollection<IncomeCategory> IncomeCategories { get; set; } = [];
    public ICollection<ExpenseCategory> ExpenseCategories { get; set; } = [];
}
