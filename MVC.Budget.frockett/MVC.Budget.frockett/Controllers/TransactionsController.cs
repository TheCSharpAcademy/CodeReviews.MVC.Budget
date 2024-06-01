using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Budget.frockett.Data;
using MVC.Budget.frockett.Models;

namespace MVC.Budget.frockett.Controllers;

public class TransactionsController : Controller
{
    private readonly BudgetContext _context;

    public TransactionsController(BudgetContext context)
    {
        _context = context;
    }

    // GET: TransactionsController
    public async Task<IActionResult> Index()
    {
        return View(await _context.Transactions.ToListAsync());
    }

    // GET: TransactionsController/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var transaction = await  _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
        
        if (transaction == null) return NotFound();

        return View(transaction);
    }

    // GET: TransactionsController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: TransactionsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Transaction transaction)
    {
        if (ModelState.IsValid)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        return View(transaction);
    }

    // GET: TransactionsController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: TransactionsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Transaction transaction)
    {
        if (id != transaction.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(e => e.Id == transaction.CategoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(transaction);
    }

    // GET: TransactionsController/Delete/5
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var transactionToDelete = await _context.Transactions.FindAsync(id);
        if (transactionToDelete != null)
        {
            _context.Transactions.Remove(transactionToDelete);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: TransactionsController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(m => m.Id == id);
        if (transaction == null)
        {
            return NotFound();
        }

        return View(transaction);
    }
}
