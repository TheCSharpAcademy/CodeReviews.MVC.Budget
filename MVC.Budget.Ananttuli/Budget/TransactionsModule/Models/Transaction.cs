using System.ComponentModel.DataAnnotations;
using Budget.CategoriesModule.Models;

namespace Budget.TransactionsModule.Models;

public class Transaction
{
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    public string Description { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Transaction must have a linked category")]
    public int CategoryId { get; set; }

    public Category Category { get; set; }
}