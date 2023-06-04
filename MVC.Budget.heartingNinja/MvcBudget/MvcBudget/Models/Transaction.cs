using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBudget.Models;

public class Transaction
{
    public int Id { get; set; }

    [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
    [StringLength(100, MinimumLength = 3)]
    [Required]
    public string Name { get; set; }

    [Range(1, 100)]  
    [Column(TypeName = "decimal(18,2)")]
    [DataType(DataType.Currency)]
    [Required]
    public decimal Amount { get; set; }
   
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0: M-d-yy}", ApplyFormatInEditMode = true)]
    [Required]
    public DateTime Date { get; set; }

    [ForeignKey("CategoryId")]
    public int CategoryId { get; set; }

    public Category Category { get; set; }
}
