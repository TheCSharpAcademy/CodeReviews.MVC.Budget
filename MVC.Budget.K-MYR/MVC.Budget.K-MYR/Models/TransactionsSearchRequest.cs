using MVC.Budget.K_MYR.Enums;

namespace MVC.Budget.K_MYR.Models;

public class TransactionsSearchRequest
{
    public int? Draw { get; set; }
    public int PageSize { get; set; } = 100;
    public bool IsPrevious { get; set; } = false;
    public int? LastId { get; set; }
    public string? LastValue { get; set; }
    public string? OrderBy { get; set; }
    public OrderDirection OrderDirection { get; set; } = OrderDirection.Ascending;
    public string? SearchString { get; set; }
    public int FiscalPlanId { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
}
