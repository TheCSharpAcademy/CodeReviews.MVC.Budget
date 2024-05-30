using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.frockett.Models;

public class Transaction
{
    public int Id { get; set; }

    [Required] public string Title { get; set; }

    [Required][Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive number")] public decimal Amount { get; set; }

    [Required] public DateTime DateTime { get; set; }

    [Required] public int CategoryId { get; set; }
    public Category Category { get; set; }

}
