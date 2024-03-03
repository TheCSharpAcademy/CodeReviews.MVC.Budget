using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.API;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(IUnitOfWork unitOfWork, ILogger<CategoriesController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetCategories()
    {
        return Ok(await _unitOfWork.CategoriesRepository.GetAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _unitOfWork.CategoriesRepository.GetByIDAsync(id);

        return category is null ? NotFound() : Ok(category);
    }

    [HttpGet("filteredByEvaluation")]
    public async Task<ActionResult<List<Category>>> GetCategoriesWithUnevaluatedTransactions()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-14);

        return Ok(await _unitOfWork.CategoriesRepository.GetCategoriesWithFilteredTransactionsAsync(
                c => c.GroupId == 2,
                q => q.OrderBy(c => c.Name),
                c => c.Transactions.Where(t => t.Evaluated == false && t.DateTime < cutoffDate)
                    .OrderByDescending(d => d.DateTime)));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PostCategory([FromBody][Bind("Name, Budget, GroupId")] CategoryPost postCategory)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var category = new Category()
        {
            Name = postCategory.Name,
            Budget = postCategory.Budget,
            GroupId = postCategory.GroupId,
        };

        _unitOfWork.CategoriesRepository.Insert(category);
        await _unitOfWork.Save();

        return CreatedAtAction(nameof(Category), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PutCategory(int id, [Bind("Name,Budget,Id")] CategoryPut category)
    {
        if (id != category.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest();

        var entity = await _unitOfWork.CategoriesRepository.GetByIDAsync(id);

        if (entity is null)
            return NotFound();

        entity.Name = category.Name;
        entity.Budget = category.Budget;
        entity.GroupId = category.GroupId;

        try
        {
            _unitOfWork.CategoriesRepository.Update(entity);
            await _unitOfWork.Save();
            return NoContent();
        }

        catch (DbUpdateConcurrencyException) when (!CategoryExists(entity.Id))
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var category = await _unitOfWork.CategoriesRepository.GetByIDAsync(id);

        if (category is null)
            return NotFound();

        try
        {
            await _unitOfWork.CategoriesRepository.DeleteAsync(id);
            await _unitOfWork.Save();
            return NoContent();
        }

        catch (DbUpdateConcurrencyException) when (!CategoryExists(id))
        {
            return NotFound();
        }
    }

    private bool CategoryExists(int id) => _unitOfWork.CategoriesRepository.GetByID(id) is null;
}
