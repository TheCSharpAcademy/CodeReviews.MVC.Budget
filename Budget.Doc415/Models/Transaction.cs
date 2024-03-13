using Budget.Doc415.Validations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.Doc415.Models;

public class Transaction
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }


    [Required, Precision(10, 2)]
    [Range(0, double.MaxValue, ErrorMessage = "The amount field is required")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true), DisplayName("Transferred Amount")]
    public decimal Amount { get; set; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true), DisplayName("Transaction Date")]
    [DateValidation(ErrorMessage = "Transaction date can not be in the future.")]
    public DateTime Date { get; set; } = DateTime.Now;

    [Required]
    [ForeignKey("Category")]
    public int RefCatId { get; set; }
    public Category Category { get; set; }

}
