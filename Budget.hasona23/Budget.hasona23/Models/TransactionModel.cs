using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Budget.hasona23.Models;

public class TransactionModel
{
    public int Id { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public float Price { get; set; }
    [StringLength(128,MinimumLength = 4)]
    public string Details { get; set; }=string.Empty;
    //Navigation
    public CategoryModel Category { get; set; }
}
public record TransactionDto(DateOnly Date, float Price,string Details,int CategoryId);