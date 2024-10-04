using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Services;
using System.ComponentModel.DataAnnotations;

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

    [HttpGet("Unevaluated")]
    public async Task<ActionResult<Transaction>> GetUnevaluatedTransactions([FromQuery][Required] int categoryid, [FromQuery] int? lastId = null, [FromQuery] DateTime? lastDate = null,[FromQuery] int pageSize = 10)
    {
        var transactions = await _transactionsService.GetUnevaluatedTransactions(categoryid, lastId, lastDate, pageSize);

        return Ok(transactions);
    }

    [HttpPost("Search")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<TransactionsSearchResponse>> GetTransactions([FromBody] TransactionsSearchRequest searchModel)
    {
        var result = await _transactionsService.GetTransactions(searchModel);

        return result.Match<ActionResult<TransactionsSearchResponse>>(
            res => Ok(res),
            err => BadRequest(err.Message));
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
    public async Task<ActionResult> PutTransaction([FromRoute] int id, [FromBody][Bind("Id, Title, DateTime, Amount, IsHappy, IsNecessary, Evaluated, EvaluatedIsHappy, EvaluatedIsNecessary, Id")] TransactionPut transactionPut)
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
            Title = transaction.Title,
            Description = transaction.Description,
            DateTime = transaction.DateTime,
            Amount = transaction.Amount,
            IsHappy = transaction.IsHappy,
            IsNecessary = transaction.IsNecessary,
            PreviousIsNecessary = transaction.PreviousIsNecessary,
            PreviousIsHappy = transaction.PreviousIsHappy,
            IsEvaluated = transaction.IsEvaluated
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
