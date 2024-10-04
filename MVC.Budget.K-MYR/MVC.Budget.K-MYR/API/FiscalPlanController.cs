using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Services;

namespace MVC.Budget.K_MYR.API;

[Route("api/[controller]")]
[ApiController]
public class FiscalPlanController : ControllerBase
{

    private readonly ILogger<FiscalPlanController> _logger;
    private readonly IFiscalPlansService _fiscalPlanService;

    public FiscalPlanController(ILogger<FiscalPlanController> logger, IFiscalPlansService budgetsService)
    {
        _logger = logger;
        _fiscalPlanService = budgetsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<FiscalPlan>>> GetFiscalPlans()
    {
        return Ok(await _fiscalPlanService.GetFiscalPlans());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FiscalPlan>> GetFiscalPlan(int id)
    {
        var fiscalPlan = await _fiscalPlanService.GetByIDAsync(id);

        return fiscalPlan is null ? NotFound() : Ok(fiscalPlan);
    }

    [HttpGet("{id:int}/MonthlyData")]
    public async Task<ActionResult<FiscalPlanMonthDTO>> GetDataByMonth([FromRoute] int id, [FromQuery] DateTime? Month)
    {
       var fiscalPlan = await _fiscalPlanService.GetByIDAsync(id);

        if(fiscalPlan is null)
        { 
            return NotFound(); 
        }
        
        var start = DateTime.UtcNow;
        FiscalPlanMonthDTO fiscalPlanDTO = await _fiscalPlanService.GetDataByMonth(fiscalPlan, Month ?? DateTime.UtcNow);
        _logger.LogInformation("{method} Duration: {duration} ms", nameof(GetDataByMonth),(DateTime.UtcNow - start).Milliseconds);

        return Ok(fiscalPlanDTO);
    }

    [HttpGet("{id:int}/YearlyData")]
    public async Task<ActionResult<FiscalPlanYearDTO>> GetDataByYear([FromRoute] int id, [FromQuery] int year)
    {
        var fiscalPlan = await _fiscalPlanService.GetByIDAsync(id);

        if (fiscalPlan is null)
        {
            return NotFound();
        }

        var start = DateTime.UtcNow;
        FiscalPlanYearDTO fiscalPlanDTO = await _fiscalPlanService.GetDataByYear(id, year);
        _logger.LogInformation("{method} Duration: {duration} ms", nameof(GetDataByYear), (DateTime.UtcNow - start).Milliseconds);


        return Ok(fiscalPlanDTO);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PostFiscalPlan([FromBody][Bind("Name")] FiscalPlanPost fiscalPlanPost)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var fiscalPlan = await _fiscalPlanService.AddFiscalPlan(fiscalPlanPost);

        return CreatedAtAction(nameof(Category), new { id = fiscalPlan.Id }, fiscalPlan);
    }

    [HttpPut("{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PutFiscalPlan([FromRoute] int id, [Bind("Name,Id")] FiscalPlanPut fiscalPlanPut)
    {
        if (id != fiscalPlanPut.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest();

        var fiscalPlan = await _fiscalPlanService.GetByIDAsync(id);

        if (fiscalPlan is null)
            return NotFound();

        try
        {
            await _fiscalPlanService.UpdateFiscalPlan(fiscalPlan, fiscalPlanPut);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException) when (!FiscalPlanExists(fiscalPlan.Id))
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteFiscalPlan([FromRoute] int id)
    {
        var fiscalPlan = await _fiscalPlanService.GetByIDAsync(id);

        if (fiscalPlan is null)
            return NotFound();

        try
        {
            await _fiscalPlanService.DeleteFiscalPlan(fiscalPlan);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException) when (!FiscalPlanExists(id))
        {
            return NotFound();
        }
    }

    private bool FiscalPlanExists(int id) => _fiscalPlanService.GetByID(id) is not null;
}
