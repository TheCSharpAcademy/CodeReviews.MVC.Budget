using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Services;

namespace MVC.Budget.K_MYR.API;

[Route("api/[controller]")]
[ApiController]
public abstract class GenericCategoriesController<T> : ControllerBase where T : Category, new()
{
    private readonly ILogger<GenericCategoriesController<T>> _logger;
    private readonly ICategoriesService _categoriesService;

    public GenericCategoriesController(ILogger<GenericCategoriesController<T>> logger, ICategoriesService categoriesService)
    {
        _logger = logger;
        _categoriesService = categoriesService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetCategories()
    {
        return Ok(await _categoriesService.GetCategories());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _categoriesService.GetByIDAsync(id);

        return category is null ? NotFound() : Ok(category);
    }

    [HttpGet("filteredByEvaluation")]
    public async Task<ActionResult<List<Category>>> GetCategoriesWithUnevaluatedTransactions()
    {
        return Ok(await _categoriesService.GetCategoriesWithUnevaluatedTransactions());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PostCategory([FromBody][Bind("Name, Budget, FiscalPlanId")] T categoryPost)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var category = await _categoriesService.AddCategory(categoryPost);

        return CreatedAtAction(nameof(Category), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PutCategory(int id, [Bind("Name,Budget,Id")] T categoryPut, [FromQuery] DateTime? month)
    {
        if (id != categoryPut.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest();

        var date = month ?? DateTime.UtcNow;

        var category = await _categoriesService.GetCategoryWithFilteredStatistics(categoryPut.Id, c => c.PreviousBudgets.Where(s => s.Month.Month == date.Month && s.Month.Year == date.Year));

        if (category is null)
            return NotFound();

        try
        {
            await _categoriesService.UpdateCategory(category, categoryPut, date);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException) when (!CategoryExists(category.Id))
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var category = await _categoriesService.GetByIDAsync(id);

        if (category is null)
            return NotFound();

        try
        {
            await _categoriesService.DeleteCategory(category);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException) when (!CategoryExists(id))
        {
            return NotFound();
        }
    }

    private bool CategoryExists(int id) => _categoriesService.GetByID(id) is not null;
}
