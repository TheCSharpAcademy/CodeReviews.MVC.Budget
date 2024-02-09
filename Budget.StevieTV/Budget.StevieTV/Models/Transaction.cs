using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Budget.StevieTV.Enums;
using Microsoft.EntityFrameworkCore;

namespace Budget.StevieTV.Models;

public class Transaction
{
    public int Id { get; set; }
    
    [DisplayName("Type")]
    public TransactionType TransactionType { get; set; }

    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
    
    public string Description { get; set; }
    
    [Range(0, (double) decimal.MaxValue)]
    [DataType(DataType.Currency)]
    [Precision(18,2)]
    public decimal Amount { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}