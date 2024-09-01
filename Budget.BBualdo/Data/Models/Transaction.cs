using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class Transaction
{
    public int Id { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Date { get; set; }

    public string Title { get; set; }

    [Range(0, (double)decimal.MaxValue)]
    [DataType(DataType.Currency)]
    [Precision(18, 2)]
    public decimal Amount { get; set; }

    [ForeignKey(nameof(Category))] public int CategoryId { get; set; }

    [DisplayName("Category")] public Category Category { get; set; }
}