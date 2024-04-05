using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.Models;

public class TransactionDTO
{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Description {get; set;} = "";
    public DateTime Date {get; set;}

    [Column(TypeName = "decimal(19, 4)")]
    [DisplayFormat(DataFormatString="{0:C}")]
    public decimal Amount {get; set;}
    
    public string Category {get; set;} = "";

    public static TransactionDTO FromTransaction(Transaction transaction)
    {
        return new TransactionDTO
        {
            Id = transaction.Id,
            Name = transaction.Name,
            Description = transaction.Description,
            Date = transaction.Date,
            Amount = transaction.Amount,
            Category = transaction.Category!.Name
        };
    }
}