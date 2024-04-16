using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.Models;

public class Transaction
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name {get; set;} = "";

    [Required]
    [StringLength(1000, MinimumLength = 3)]
    public string Description {get; set;} = "";

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date {get; set;}

    [Range(0, int.MaxValue, MinimumIsExclusive = true), DataType(DataType.Currency), Column(TypeName = "decimal(19, 4)")]
    [DisplayFormat(DataFormatString="{0:C}")]
    public decimal Amount {get; set;}
    
    [Required]
    public Category Category {get; set;} = new Category();

    public static Transaction FromDTO (TransactionDTO transactionDTO)
    {
        return new Transaction 
        {   
            Id = transactionDTO.Id,
            Name = transactionDTO.Name,
            Description = transactionDTO.Description,
            Date = transactionDTO.Date,
            Amount = transactionDTO.Amount,
        };
    }
}