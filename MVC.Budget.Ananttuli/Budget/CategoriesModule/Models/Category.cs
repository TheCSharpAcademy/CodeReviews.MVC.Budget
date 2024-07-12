using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Budget.TransactionsModule.Models;

namespace Budget.CategoriesModule.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 2)]
    public string Name { get; set; }
}

