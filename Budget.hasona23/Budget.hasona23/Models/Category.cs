using System.ComponentModel.DataAnnotations;

namespace Budget.hasona23.Models;

public class Category
{
    #region Constructors
    public Category(){}
    public Category(string name)
    {
        Name = name;
    }

    #endregion
    #region Fields

    public int Id { get; set; }
    [StringLength(32, MinimumLength = 4)] public string Name { get; set; } = string.Empty;
    public ICollection<Transaction> Transactions { get; set; }

    #endregion
}