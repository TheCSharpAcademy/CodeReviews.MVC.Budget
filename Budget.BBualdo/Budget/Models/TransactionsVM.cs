using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Budget.Models;

public class TransactionsVM
{
    public TransactionsVM()
    {
    }
    
    public TransactionsVM(List<Category> categories)
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

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Date { get; set; }
    
    public string Title { get; set; }
    
    [Range(0, (double) decimal.MaxValue)]
    [DataType(DataType.Currency)]
    [Precision(18,2)]
    public decimal Amount { get; set; }
    
    [DisplayName("Category")]
    public int CategoryId { get; set; }

    public List<SelectListItem> Categories { get; set; } = new();
}