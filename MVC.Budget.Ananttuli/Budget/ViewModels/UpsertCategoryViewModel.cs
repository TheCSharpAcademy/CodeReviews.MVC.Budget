using Budget.CategoriesModule.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.ViewModels;

public class UpsertCategoryViewModel
{
    public Category? Category { get; set; }

    public SelectList BudgetDurations { get; set; } = new SelectList
        (
            Enum.GetValues(typeof(BudgetDuration)).Cast<BudgetDuration>(),
            BudgetDuration.Monthly
        );
}