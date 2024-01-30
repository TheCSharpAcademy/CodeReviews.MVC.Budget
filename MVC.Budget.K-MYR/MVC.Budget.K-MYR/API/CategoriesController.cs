using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Repositories;

namespace MVC.Budget.K_MYR.API;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoriesRepository _repo;

    public CategoriesController(ICategoriesRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> Get()
    {
        return Ok(await _repo.GetCategories());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> Get(int id)
    {
        var category = await _repo.GetCategoryAsync(id);

        return category is null ? NotFound() : Ok(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Post([FromBody][Bind("Name")] Category category)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        await _repo.AddCategoryAsnyc(category);

        return CreatedAtAction(nameof(Get), new { id = category.Id }, category);

    }

    [HttpPut("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Put(int id, [Bind("Name,Id")] Category category)
    {
        if(id != category.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest();

        var entity = await _repo.GetCategoryAsync(id);

        if (entity is null)
            return NotFound();

        entity.Name = category.Name;

        try
        {
            await _repo.UpdateCategoryAsnyc(entity);
            return NoContent();
        }

        catch (DbUpdateConcurrencyException) when (!CategoryExists(entity.Id))
        {
            return NotFound();
        }

    }

    private bool CategoryExists(int id) => _repo.GetCategory(id) is null;
    

    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id)
    {
        var category = await _repo.GetCategoryAsync(id);

        if (category is null)
            return NotFound();

        try
        {
            await _repo.DeleteCategoryAsnyc(id);
            return NoContent();
        }

        catch (DbUpdateConcurrencyException) when (!CategoryExists(id))
        {
            return NotFound();
        }

    }
}
