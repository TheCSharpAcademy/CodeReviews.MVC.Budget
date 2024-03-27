using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.K_MYR.Models;

public class CategoryBudget
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    [DataType(DataType.Currency)]
    [Precision(19, 4)]
    public decimal Budget { get; set; } = 0;
    public DateTime Month { get; set; }
}
