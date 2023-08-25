using System.ComponentModel.DataAnnotations;

namespace kmakai.Budget.Models.ViewModels;

public class AddTransactionViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    public decimal Amount { get; set; }

    [Required]
    [Display(Name = "Transaction Type")]
    public int TransactionTypeId { get; set; }

    [Required]
    public TransactionType TransactionType { get; set; } = null!;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;
}
