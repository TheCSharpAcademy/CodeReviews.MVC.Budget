using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Budget.hasona23.Models;

public class TransactionModel
{
    public int Id { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    [DataType(DataType.Currency)]
    [Range(0, double.MaxValue)]    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }    
    [StringLength(128,MinimumLength = 4)]
    public string Details { get; set; }=string.Empty;
    //Navigation
    public CategoryModel Category { get; set; }
}
public record TransactionDto(DateOnly Date, decimal Price,string Details,int CategoryId);