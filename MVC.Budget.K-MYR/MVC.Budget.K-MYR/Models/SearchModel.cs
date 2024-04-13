using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Budget.K_MYR.Models;

public class SearchModel
{
    public string? SearchString { get; set; }
    public SelectList? Categories { get; set; }
    public int? CategoryId { get; set; }
    public int MinAmount { get; set; }
    public int MaxAmount { get; set; }
    public string? AmountRange { get; set; }
    public DateTime MinDate { get; set; }
    public DateTime MaxDate { get; set; }
}
