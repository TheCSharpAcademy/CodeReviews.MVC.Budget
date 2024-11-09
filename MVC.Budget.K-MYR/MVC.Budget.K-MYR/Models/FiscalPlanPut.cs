using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVC.Budget.K_MYR.Models;
[BindRequired]
public class FiscalPlanPut
{
    [JsonRequired]
    public int Id { get; set; }
    [JsonRequired, Required]
    [StringLength(50, MinimumLength = 0, ErrorMessage = "'Name' must be between 1 and 50 characters.")]
    public string Name { get; set; } = string.Empty;
}
