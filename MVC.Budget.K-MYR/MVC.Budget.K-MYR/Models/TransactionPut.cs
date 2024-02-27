using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.K_MYR.Models;

public class TransactionPut
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    [Display(Name = "Date & Time")]
    [DataType(DataType.DateTime)]
    public DateTime DateTime { get; set; }
    [DataType(DataType.Currency)]
    [Precision(19, 4)]
    public decimal Amount { get; set; }
    public bool IsHappy { get; set; }
    public bool IsNecessary { get; set; }
    public bool Evaluated { get; set; }
    public bool EvaluatedIsHappy { get; set; }
    public bool EvaluatedIsNecessary { get; set; }
    public int CategoryId { get; set; }
}
