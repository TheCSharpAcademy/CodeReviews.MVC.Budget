using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Budget.CategoriesModule.Models;
using Budget.Data;
using Budget.TransactionsModule.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using Budget.ViewModels;

namespace Budget.Controllers;

public class CategoriesController : Controller
{
    private readonly ILogger<CategoriesController> _logger;
    private readonly BudgetDb _db;

    public CategoriesController(ILogger<CategoriesController> logger, BudgetDb budgetDb)
    {
        _logger = logger;
        _db = budgetDb;
    }

    public PartialViewResult CreateCategoryModal()
    {
        var model = new UpsertCategoryViewModel { Category = new Category() };


        return PartialView("_PartialCreateCategoryModalView", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<NoContent> CreateCategory([Bind("Category", "Category.Name", "Category.Duration", "Category.Budget")] UpsertCategoryViewModel model)
    {
        _db.Categories.Add(new Category
        {
            Name = model.Category.Name,
            Duration = model.Category.Duration,
            Budget = model.Category.Budget
        });

        await _db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public async Task<PartialViewResult> EditCategoryModal(int? id)
    {
        var category = await _db.Categories.FindAsync(id);

        var model = new UpsertCategoryViewModel
        {
            Category = category,
            BudgetDurations = new SelectList
                (
                    Enum.GetValues(typeof(BudgetDuration)).Cast<BudgetDuration>(),
                    category?.Duration ?? BudgetDuration.Monthly
                )
        };

        return PartialView("_PartialEditCategoryModalView", model);
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<Results<NotFound, Ok<Category>>> UpdateCategory(
        [Bind("Category", "Category.Id", "Category.Name", "Category.Duration", "Category.Budget")]
            UpsertCategoryViewModel model
        )
    {
        var foundCategory = await _db.Categories.FindAsync(model.Category.Id);

        if (foundCategory is null)
        {
            return TypedResults.NotFound();
        }

        foundCategory.Name = model.Category.Name;
        foundCategory.Duration = model.Category.Duration;
        foundCategory.Budget = model.Category.Budget;

        _db.Categories.Update(foundCategory);

        await _db.SaveChangesAsync();

        return TypedResults.Ok(foundCategory);
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<Results<NotFound, NoContent>> Delete(int id)
    {
        var foundCategory = await _db.Categories.FindAsync(id);

        if (foundCategory is null)
        {
            return TypedResults.NotFound();
        }

        _db.Categories.Remove(foundCategory);

        await _db.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}
