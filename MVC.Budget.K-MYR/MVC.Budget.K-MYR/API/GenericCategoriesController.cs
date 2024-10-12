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
    public async Task<ActionResult<Category>> GetCategory([FromRoute] int id)
    {
        var category = await _categoriesService.GetByIDAsync(id);

        return category is null ? NotFound() : Ok(category);
    }

    [HttpGet("{id:int}/MonthlyData")]
    public async Task<ActionResult<FiscalPlanMonthDTO>> GetDataByMonth([FromRoute] int id, [FromQuery] DateTime? Month)
    {
        CategoryMonthDTO? categoryDTO = await _categoriesService.GetCategoryDataByMonth(id, Month ?? DateTime.UtcNow);

        if (categoryDTO is null)
        {
            return NotFound();
        }

        return Ok(categoryDTO);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PostCategory([FromBody] CategoryPost categoryPost)
    {
        var category = await _categoriesService.AddCategory<T>(categoryPost);
        _logger.LogCritical("Category Id {id} Type {type}", category.Id, category.CategoryType);

        return CreatedAtAction(nameof(Category), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PutCategory([FromRoute] int id, CategoryPut categoryPut, [FromQuery] DateTime? month)
    {
        if (id != categoryPut.Id)
            return BadRequest();

        var date = month ?? DateTime.UtcNow;
        var cutOffDate = new DateTime(date.Year, date.Month, 1);

        var category = await _categoriesService.GetCategoryWithBudgetLimit(categoryPut.Id, cutOffDate);

        if (category is null)
        {
            return NotFound();
        }

        try
        {
            await _categoriesService.UpdateCategory(category, categoryPut, cutOffDate);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException) when (!CategoryExists(category.Id))
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteCategory([FromRoute] int id)
    {
        var category = await _categoriesService.GetByIDAsync(id);

        if (category is null)
        {
            return NotFound();
        }

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
