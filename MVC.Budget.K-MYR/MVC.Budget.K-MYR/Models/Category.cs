using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.K_MYR.Models;

public abstract class Category
{
    public int Id { get; set; }
    public int CategoryType { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string? Name { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    [Precision(19,4)]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal Budget { get; set; }
    public int FiscalPlanId { get; set; }
    public virtual FiscalPlan? FiscalPlan { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; } = [];
    public virtual ICollection<CategoryBudget> PreviousBudgets { get; set;  } = [];
}