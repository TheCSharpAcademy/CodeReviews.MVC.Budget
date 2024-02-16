using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.API;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoriesController> _logger;

    public TransactionsController(IUnitOfWork unitOfWork, ILogger<CategoriesController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetTransactions()
    {
        return Ok(await _unitOfWork.TransactionsRepository.GetAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetTransaction(int id)
    {
        var transaction = await _unitOfWork.TransactionsRepository.GetByIDAsync(id);

        return transaction is null ? NotFound() : Ok(transaction);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PostTransaction([FromBody][Bind("Title, Amount, IsHappy, IsNecessary, CategoryId")] TransactionPost postTransaction)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var transaction = new Transaction()
        {
            Title = postTransaction.Title,
            Amount = postTransaction.Amount,
            IsHappy = postTransaction.IsHappy,
            IsNecessary = postTransaction.IsNecessary,
            CategoryId = postTransaction.CategoryId,            
        };

        _unitOfWork.TransactionsRepository.Insert(transaction);
        await _unitOfWork.Save();

        return CreatedAtAction(nameof(Transaction), new { id = transaction.Id }, transaction);
    }

    [HttpPut("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PutTransaction(int id, [FromBody][Bind("Title, Amount, IsHappy, IsNecessary, Id")] Transaction transaction)
    {
        if (id != transaction.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest();

        var entity = await _unitOfWork.TransactionsRepository.GetByIDAsync(id);

        if (entity is null)
            return NotFound();

        entity.Title = transaction.Title;
        entity.Amount = transaction.Amount;
        entity.IsHappy = transaction.IsHappy;
        entity.IsNecessary = transaction.IsNecessary;

        try
        {
            _unitOfWork.TransactionsRepository.Update(entity);
            await _unitOfWork.Save();
            return NoContent();
        }

        catch (DbUpdateConcurrencyException) when (!TransactionExists(entity.Id))
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteTransaction(int id)
    {
        var transaction = await _unitOfWork.TransactionsRepository.GetByIDAsync(id);

        if (transaction is null)
            return NotFound();

        try
        {
            await _unitOfWork.TransactionsRepository.DeleteAsync(id);
            await _unitOfWork.Save();
            return NoContent();
        }

        catch (DbUpdateConcurrencyException) when (!TransactionExists(id))
        {
            return NotFound();
        }
    }

    private bool TransactionExists(int id) => _unitOfWork.TransactionsRepository.GetByID(id) is null;
}
