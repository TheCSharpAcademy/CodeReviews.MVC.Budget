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


    [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
    [DataType(DataType.Currency)]
    public decimal? Budget { get; set; }

    [DisplayName("Budget duration")]
    public BudgetDuration? Duration { get; set; }
}

