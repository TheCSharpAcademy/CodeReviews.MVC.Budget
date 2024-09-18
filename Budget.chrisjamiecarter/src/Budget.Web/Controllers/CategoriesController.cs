using Budget.Application.Services;
using Budget.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Web.Controllers;

/// <summary>
/// Manages the Category-related actions for the Presentation layer.
/// This controller handles the CRUD operations.
/// </summary>
public class CategoriesController : Controller
{
    #region Fields

    private readonly ICategoryService _categoryService;

    #endregion
    #region Constructors

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    #endregion
    #region Methods

    // GET: Categories
    public async Task<IActionResult> Index()
    {
        var entities = await _categoryService.ReturnAsync(orderBy: o => o.OrderBy(k => k.Name));
        var categories = entities.Select(x => new CategoryViewModel(x));

        return View(categories);
    }

    // GET: Categories/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (!id.HasValue)
        {
            return NotFound();
        }

        var entity = await _categoryService.ReturnAsync(id.Value);
        if (entity is null)
        {
            return NotFound();
        }

        var category = new CategoryViewModel(entity);
        return Ok(category);
    }

    // GET: Categories/Create
    public IActionResult Create()
    {
        var viewModel = new CategoryViewModel();

        return PartialView("_CreatePartial", viewModel);
    }

    // POST: Categories/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] CategoryViewModel category)
    {
        if (ModelState.IsValid && await IsDuplicateCategoryName(category.Id, category.Name))
        {
            ModelState.AddModelError("Name", "A Categeory with that Name already exists.");
        }

        if (ModelState.IsValid)
        {
            category.Id = Guid.NewGuid();
            await _categoryService.CreateAsync(category.MapToDomain());
            return Json(new { success = true });
        }

        return PartialView("_CreatePartial", category);
    }

    // GET: Categories/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (!id.HasValue)
        {
            return NotFound();
        }

        var entity = await _categoryService.ReturnAsync(id.Value);
        if (entity is null)
        {
            return NotFound();
        }

        var viewModel = new CategoryViewModel(entity);
        return PartialView("_EditPartial", viewModel);
    }

    // POST: Categories/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] CategoryViewModel category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid && await IsDuplicateCategoryName(category.Id, category.Name))
        {
            ModelState.AddModelError("Name", "A Categeory with that Name already exists.");
        }

        if (ModelState.IsValid)
        {
            await _categoryService.UpdateAsync(category.MapToDomain());
            return Json(new { success = true });
        }

        return PartialView("_EditPartial", category);
    }

    // GET: Categories/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (!id.HasValue)
        {
            return NotFound();
        }

        var entity = await _categoryService.ReturnAsync(id.Value);
        if (entity is null)
        {
            return NotFound();
        }

        var category = new CategoryViewModel(entity);
        return PartialView("_DeletePartial", category);
    }

    // POST: Categories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _categoryService.DeleteAsync(id);
        return Json(new { success = true });
    }

    private async Task<bool> IsDuplicateCategoryName(Guid id, string name)
    {
        var categories = await _categoryService.ReturnAsync();

        var match = categories.FirstOrDefault(c => c.Name!.Equals(name, StringComparison.CurrentCultureIgnoreCase));

        return match is not null && match.Id != id;
    }

    #endregion
}
