using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Budget.StevieTV.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Budget.StevieTV.Models;

public class TransactionViewModel
{

    public TransactionViewModel()
    {
    }
    
    public TransactionViewModel(List<Category> categories)
    {
        Categories = new List<SelectListItem>();
        
        foreach (var category in categories)
        {
            Categories.Add(new SelectListItem()
            {
                Text = category.Name,
                Value = category.Id.ToString()
            });
        }
    }
    
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
    
    [DisplayName("Category")]
    public int CategoryId { get; set; }

    public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
}