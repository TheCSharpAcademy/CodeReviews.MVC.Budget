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
    [EnumDataType(typeof(TransactionType))]
    public TransactionType TransactionType { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Date { get; set; }
    
    public string Description { get; set; }
    
    [Range(0, (double) decimal.MaxValue)]
    [DataType(DataType.Currency)]
    [Precision(18,2)]
    public decimal Amount { get; set; }
    
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    [DisplayName("Category")]
    public Category Category { get; set; }
}