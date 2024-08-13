using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Models.ViewModels;
using MVC.Budget.K_MYR.Services;

namespace MVC.Budget.K_MYR.API;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController(ILogger<TransactionsController> logger, ITransactionsService transactionsService) : ControllerBase
{
    private readonly ILogger<TransactionsController> _logger = logger;
    private readonly ITransactionsService _transactionsService = transactionsService;

    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetTransaction([FromRoute] int id)
    {
        var transaction = await _transactionsService.GetByIDAsync(id);

        return transaction is null ? NotFound() : Ok(transaction);
    }

    [HttpPost("search")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<List<Transaction>>> GetTransactions([FromBody] TransactionsSearchModel? searchModel)
    {
        return Ok(await _transactionsService.GetTransactions(searchModel?.FiscalPlanId, searchModel?.CategoryId, searchModel?.SearchString, searchModel?.MinDate, searchModel?.MaxDate, searchModel?.MinAmount, searchModel?.MaxAmount));

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PostTransaction([FromBody][Bind("Title, DateTime, Amount, IsHappy, IsNecessary, CategoryId")] TransactionPost transactionPost)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var transaction = await _transactionsService.AddTransaction(transactionPost);

        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
    }

    [HttpPut("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PutTransaction([FromRoute] int id, [FromBody][Bind("Title, DateTime, Amount, IsHappy, IsNecessary, Evaluated, EvaluatedIsHappy, EvaluatedIsNecessary, Id")] TransactionPut transactionPut)
    {
        if (id != transactionPut.Id || !ModelState.IsValid)
            return BadRequest();

        var transaction = await _transactionsService.GetByIDAsync(id);

        if (transaction is null)
            return NotFound();

        try
        {
            await _transactionsService.UpdateTransaction(transaction, transactionPut);
            return NoContent();
        }

        catch (DbUpdateConcurrencyException) when (!TransactionExists(transaction.Id))
        {
            return NotFound();
        }
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchTransaction([FromRoute] int id, [FromBody] JsonPatchDocument<TransactionPut> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest();

        var transaction = await _transactionsService.GetByIDAsync(id);

        if (transaction is null)
            return NotFound();

        TransactionPut transactionToPatch = new()
        {
            Id = transaction.Id,
            CategoryId = transaction.CategoryId,
            Title = transaction.Title,
            Description = transaction.Description,
            DateTime = transaction.DateTime,
            Amount = transaction.Amount,
            IsHappy = transaction.IsHappy,
            IsNecessary = transaction.IsNecessary,
            PreviousIsNecessary = transaction.PreviousIsNecessary,
            PreviousIsHappy = transaction.PreviousIsHappy,
            Evaluated = transaction.Evaluated
        };

        patchDoc.ApplyTo(transactionToPatch, ModelState);

        if (transactionToPatch.Id != transaction.Id || !ModelState.IsValid)
            return BadRequest();

        try
        {
            await _transactionsService.UpdateTransaction(transaction, transactionToPatch);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException) when (!TransactionExists(transaction.Id))
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteTransaction([FromRoute] int id)
    {
        var transaction = await _transactionsService.GetByIDAsync(id);

        if (transaction is null)
            return NotFound();

        try
        {
            await _transactionsService.DeleteTransaction(transaction);
            return NoContent();
        }

        catch (DbUpdateConcurrencyException) when (!TransactionExists(id))
        {
            return NotFound();
        }
    }

    private bool TransactionExists(int id) => _transactionsService.GetByID(id) is not null;
}
