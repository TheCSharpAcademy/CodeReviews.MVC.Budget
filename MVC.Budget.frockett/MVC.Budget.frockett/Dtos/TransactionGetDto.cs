using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.frockett.Dtos;

public class TransactionGetDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    [Required, Precision(10, 2)][Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive number")] public decimal Amount { get; set; }
    public DateTime DateTime { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}
