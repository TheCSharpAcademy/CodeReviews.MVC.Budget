using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.jollejonas.Models;

public class Transaction
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal Amount { get; set; }
    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }
    [Required(ErrorMessage = "Category is required")]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
