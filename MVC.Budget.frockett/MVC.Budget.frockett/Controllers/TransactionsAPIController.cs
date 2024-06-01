using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Budget.frockett.Data;
using MVC.Budget.frockett.Models;
using MVC.Budget.frockett.Dtos;

namespace MVC.Budget.frockett.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsAPIController : ControllerBase
{
    private readonly BudgetContext _context;

    public TransactionsAPIController(BudgetContext context)
    {
        _context = context;
    }

    // GET: api/Transactions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionGetDto>>> GetTransactions()
    {
        var transactions = await _context.Transactions.Include(t => t.Category).ToListAsync();

        var transactionsToReturn = new List<TransactionGetDto>();

        foreach(var transaction in transactions)
        {
            transactionsToReturn.Add(new TransactionGetDto
            {
                Id = transaction.Id,
                Title = transaction.Title,
                Amount = transaction.Amount,
                DateTime = transaction.DateTime,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category.Name
            });
        }

        return transactionsToReturn;
    }

    // GET: api/Transactions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetTransaction(int id)
    {
        var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id); 

        if (transaction == null)
        {
            return NotFound();
        }

        return transaction;
    }

    // PUT: api/Transactions/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
    {
        if (id != transaction.Id)
        {
            return BadRequest();
        }

        var category = await _context.Categories.FindAsync(transaction.CategoryId);
        if (category == null)
        {
            return NotFound($"Category with ID {transaction.CategoryId} not found.");
        }


        _context.Entry(transaction).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TransactionExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Transactions
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Transaction>> PostTransaction(TransactionPostDto transactionDto)
    {
        var category = await _context.Categories.FindAsync(transactionDto.CategoryId);
        if (category == null)
        {
            return NotFound($"Category with ID {transactionDto.CategoryId} not found.");
        }

        var transaction = new Transaction
        {
            Title = transactionDto.Title,
            Amount = transactionDto.Amount,
            DateTime = transactionDto.DateTime,
            CategoryId = transactionDto.CategoryId,
            Category = category
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
    }

    // DELETE: api/Transactions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TransactionExists(int id)
    {
        return _context.Transactions.Any(e => e.Id == id);
    }
}
