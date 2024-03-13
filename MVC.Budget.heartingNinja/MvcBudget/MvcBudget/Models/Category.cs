using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcBudget.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public List<Transaction> Transactions { get; set; }
}
