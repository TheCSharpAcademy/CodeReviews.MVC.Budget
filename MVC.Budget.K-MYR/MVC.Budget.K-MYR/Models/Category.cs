using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.K_MYR.Models;

public abstract class Category
{
    public int Id { get; set; }
    public int CategoryType { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "'Name' must be between 1 and 50 characters.")]
    public string Name { get; set; } = string.Empty;
    [DataType(DataType.Currency)]
    [Range(0.0, 100000000000000, ErrorMessage = $"'Amount' must be between 0 and 100000000000000.")] 
    public decimal Budget { get; set; }
    public int FiscalPlanId { get; set; }
    public FiscalPlan? FiscalPlan { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<CategoryBudget> PreviousBudgets { get; set;  } = [];
}