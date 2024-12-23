using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.jollejonas.Models;

public class TransactionCategoryViewModel
{
    public List<Transaction> Transaction { get; set; }
    public List<Category> Categories { get; set; }
    public SelectList? CategoriesList { get; set; }
    public int? CategoryId { get; set; }
    public string? SearchString { get; set; }
    public SelectList? YearList { get; set; }
    public int? Year { get; set; }
    public SelectList? MonthList { get; set; }
    public int? Month { get; set; }

    public TransactionCategoryViewModel()
    {
        YearList = new SelectList(Enumerable.Range(DateTime.Now.Year - 10, 21).Select(y => new SelectListItem
        {
            Value = y.ToString(),
            Text = y.ToString()
        }).ToList(), "Value", "Text");

        MonthList = new SelectList(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
            .Where(m => !string.IsNullOrEmpty(m))
            .Select((m, index) => new SelectListItem
            {
                Value = (index + 1).ToString(),
                Text = m
            }).ToList(), "Value", "Text");
    }
}
