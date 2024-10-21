using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVC.Budget.K_MYR.Models;
[BindRequired]
public class CategoryPut
{
    [JsonRequired]
    public int Id { get; set; }
    [JsonRequired, Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "'Name' must be between 1 and 50 characters.")]
    public string Name { get; set; } = String.Empty;
    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    [Range(0.0, 100000000000000, ErrorMessage = $"'Amount' must be between 0 and 100000000000000.")] 
    [JsonRequired]
    public decimal Budget { get; set; }
    [JsonRequired]
    public int FiscalPlanId { get; set; }
}
