using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.K_MYR.Models;

public class CategoryStatistic
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    [DataType(DataType.Currency)]
    [Precision(19, 4)]
    public decimal Budget { get; set; }
    public DateTime Month { get; set; }
    [DataType(DataType.Currency)]
    [Precision(19, 4)]
    public decimal TotalSpent { get; set; } = 0;
    [DataType(DataType.Currency)]
    [Precision(19, 4)]
    public decimal Overspending { get; set; } = 0;

    public void ChangeTotalSpent(decimal change)
    {
        TotalSpent += change;
        Overspending = Math.Max(0, TotalSpent - Budget);
    }
}
