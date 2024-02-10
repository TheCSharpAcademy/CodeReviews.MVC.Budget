using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.K_MYR.Models;

public class CategoryPost
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string? Name { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    [Precision(19, 4)]
    public decimal Budget { get; set; }
    [Required]
    public int GroupId { get; set; }
}
