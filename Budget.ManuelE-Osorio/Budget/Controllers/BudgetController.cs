using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Budget.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using System;

namespace Budget.Controllers;

public class BudgetController(BudgetContext context) : Controller
{
    private readonly BudgetContext _context = context;

    public async Task<IActionResult> Index(string? transactionName, string? categoryName, DateOnly? transactionDate)
    {
        if (_context.Transactions == null)
            return Problem("Entity set 'MvcMovieContext.Movie'  is null.");

        var transactionQuery = from m in _context.Transactions
            select m;
  
        transactionQuery = transactionQuery.Include(p => p.Category);

        if(transactionName is not null)
            transactionQuery = transactionQuery.Where( p => p.Name.Contains(transactionName) == true);

        if(categoryName is not null)
            transactionQuery = transactionQuery.Where( p => p.Category.Name.Contains(categoryName) == true);

        if(transactionDate is not null)
            transactionQuery = transactionQuery.Where( p => DateOnly.FromDateTime(p.Date) == transactionDate);

        var viewModel = new IndexViewModel 
        {
            Categories = await _context.Categories.ToListAsync(),
            Transactions = await transactionQuery.ToListAsync()
        };
        return View( viewModel );
    }

    [HttpPost]
    [Route("Budget/Transactions/Create/")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTransaction(
        [Bind("Name,Description,Date,Amount,Category"), FromBody] TransactionDTO transactionDto)
    {
        if (ModelState.IsValid && _context.Categories.Any( p => p.Name == transactionDto.Category ))
        {
            var transaction = Transaction.FromDTO(transactionDto);
            transaction.Category = _context.Categories.FirstOrDefault( p => p.Name == transactionDto.Category)!;
            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();
            return Created($"Budget/Transactions/{transaction.Id}", transaction);
        }
        return BadRequest();
    }

    [HttpDelete]
    [Route("Budget/Transactions/Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction != null)
            _context.Transactions.Remove(transaction);
        else
            return NotFound();
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    [Route("Budget/Transactions/Update/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateTransaction(int id, 
        [Bind("Id,Name,Description,Date,Amount,Category"), FromBody] TransactionDTO transactionDto)
    {

        if(!ModelState.IsValid || id != transactionDto.Id)
            return BadRequest();

        Transaction transaction;

        if ( _context.Transactions.Any( p => p.Id == transactionDto.Id ))
        {
            if(_context.Categories.Any( p => p.Name == transactionDto.Category))
            {
                transaction = Transaction.FromDTO(transactionDto);
                transaction.Category = _context.Categories.FirstOrDefault( p => p.Name == transactionDto.Category)!;
            }
            else
                return BadRequest();

            try
            {
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch(DBConcurrencyException)
            {
                return StatusCode(500);
            }
        }
        else
            return NotFound();

        return Ok(transaction);
    }

    [HttpPost]
    [Route("Budget/Categories/Create/")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory([Bind("Name"), FromBody] Category category)
    {
        if (ModelState.IsValid && !_context.Categories.Any( p => p.Name == category.Name))
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Created($"Budget/Categories/{category.Id}", category);
        }
        return BadRequest();
    }

    [HttpDelete]
    [Route("Budget/Categories/Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category =  _context.Categories.Find( id ) ;
        if (category != null)
            _context.Remove(category);
        else
            return NotFound();
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    [Route("Budget/Categories/Update/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCategory(int id, 
        [Bind("Id,Name"), FromBody] Category category)
    {
        if( id != category.Id)
            return BadRequest();

        if ( ModelState.IsValid && _context.Categories.Any( p => p.Id == category.Id ) && 
            !_context.Categories.Any( p => p.Name == category.Name))
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
            catch(DBConcurrencyException)
            {
                return StatusCode(500);
            }
        }
        else
            return NotFound();

        return Ok(category);
    }
}