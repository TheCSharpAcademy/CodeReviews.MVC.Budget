using System.ComponentModel.DataAnnotations;

namespace Budget.hasona23.Models;

public class Transaction
{
    #region Constructors
    
    public Transaction(){}

    public Transaction(string description, float price, DateTime date, bool isProfit,Category category)
    {
        Description = description;
        Price = price;
        Date = date;
        IsProfit = isProfit;
        Category = category;
    }
    #endregion
    #region Fields
    public int Id { get; set; }

    [StringLength(64,MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;
    public float Price { get; set; }
    public bool IsProfit { get; set; }
    public DateTime Date { get; set; }
    public Category Category { get; set; }

    #endregion
    
}