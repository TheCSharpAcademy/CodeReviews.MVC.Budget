using Budget.CategoriesModule.Models;
using Budget.Data;
using Budget.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Controllers;

public class CategoriesController : Controller
{
    private readonly BudgetDb _db;
    private readonly ILogger<CategoriesController> _logger;

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
    public async Task<NoContent> CreateCategory([Bind("Category", "Category.Name")] UpsertCategoryViewModel model)
    {
        _db.Categories.Add(new Category
        {
            Name = model.Category!.Name
        });

        await _db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public async Task<PartialViewResult> EditCategoryModal(int? id)
    {
        var category = await _db.Categories.FindAsync(id);

        var model = new UpsertCategoryViewModel
        {
            Category = category
        };

        return PartialView("_PartialEditCategoryModalView", model);
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<Results<NotFound, Ok<Category>>> UpdateCategory(
        [Bind("Category", "Category.Id", "Category.Name")]
        UpsertCategoryViewModel model
    )
    {
        var foundCategory = await _db.Categories.FindAsync(model.Category.Id);

        if (foundCategory is null) return TypedResults.NotFound();

        foundCategory.Name = model.Category.Name;

        _db.Categories.Update(foundCategory);

        await _db.SaveChangesAsync();

        return TypedResults.Ok(foundCategory);
    }

    [HttpDelete]
    public async Task<Results<NotFound, NoContent>> Delete(int id)
    {
        var foundCategory = await _db.Categories.FindAsync(id);

        if (foundCategory is null) return TypedResults.NotFound();

        _db.Categories.Remove(foundCategory);

        await _db.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}