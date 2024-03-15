using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;
using MVC.Budget.K_MYR.Services;

namespace MVC.Budget.K_MYR.API;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ILogger<TransactionsController> _logger;
    private readonly ITransactionsService _transactionsService;


    public TransactionsController(ILogger<TransactionsController> logger, ITransactionsService transactionsService)
    {
        _logger = logger;
        _transactionsService = transactionsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetTransactions()
    {
        return Ok(await _transactionsService.GetTransactions());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetTransaction(int id)
    {
        var transaction = await _transactionsService.GetByIDAsync(id);

        return transaction is null ? NotFound() : Ok(transaction);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PostTransaction([FromBody][Bind("Title, DateTime, Amount, IsHappy, IsNecessary, CategoryId")] TransactionPost transactionPost)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var category = await _transactionsService.GetCategoryWithFilteredStatistics(transactionPost.CategoryId, s => s.Month.Month == transactionPost.DateTime.Month && s.Month.Year == transactionPost.DateTime.Year);

        if (category is null)
            return NotFound();

        var transaction = await _transactionsService.AddTransaction(transactionPost, category);

        return CreatedAtAction(nameof(Transaction), new { id = transaction.Id }, transaction);
    }

    [HttpPut("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PutTransaction(int id, [FromBody][Bind("Title, DateTime, Amount, IsHappy, IsNecessary, Evaluated, EvaluatedIsHappy, EvaluatedIsNecessary, Id")] TransactionPut transactionPut)
    {
        if (id != transactionPut.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest();

        var transaction = await _transactionsService.GetByIDAsync(id);

        if (transaction is null)
            return NotFound();
        
        var category = await _transactionsService.GetCategoryWithFilteredStatistics(transactionPut.CategoryId, s => s.Month.Month == transactionPut.DateTime.Month && s.Month.Year == transactionPut.DateTime.Year);      

        if (category is null)
            return NotFound();      

        try
        {
            await _transactionsService.UpdateTransaction(category, transaction, transactionPut);
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

        if(transactionToPatch.Id != transaction.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest();        

        var category = await _transactionsService.GetCategoryWithFilteredStatistics(transactionToPatch.CategoryId, s => s.Month.Month == transactionToPatch.DateTime.Month && s.Month.Year == transactionToPatch.DateTime.Year);

        if (category is null)
            return NotFound();

        try
        {
            await _transactionsService.UpdateTransaction(category, transaction, transactionToPatch);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException) when (!TransactionExists(transaction.Id))
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteTransaction(int id)
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
