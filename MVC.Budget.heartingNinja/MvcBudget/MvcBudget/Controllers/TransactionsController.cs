using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBudget.Data;
using MvcBudget.Models;

namespace MvcBudget.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly MvcBudgetContext _context;

        public TransactionsController(MvcBudgetContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(int? selectedCategoryId, string searchString, DateTime? selectedDate)
        {
            IQueryable<Category> categoryQuery = _context.Category.OrderBy(m => m.Name);
            IQueryable<Transaction> transactions = _context.Transaction.Include(t => t.Category);

            if (!String.IsNullOrEmpty(searchString))
            {
                transactions = transactions.Where(t => t.Name.Contains(searchString));
            }

            if (selectedCategoryId.HasValue)
            {
                transactions = transactions.Where(t => t.CategoryId == selectedCategoryId.Value);
            }

            if (selectedDate.HasValue)
            {
                transactions = transactions.Where(t => t.Date.Date == selectedDate.Value.Date);
            }

            var viewModel = new CategoryViewModel
            {
                Categories = await categoryQuery.Distinct().ToListAsync(),
                Transactions = await transactions.ToListAsync(),
                SelectedCategoryId = selectedCategoryId,
                SearchString = searchString,
                SelectedDate = selectedDate // Assign the selected date
            };

            return View(viewModel);
        }



        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Amount,Date,CategoryId")] Transaction transaction)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", transaction.CategoryId);
            //return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Amount,Date,CategoryId")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!TransactionExists(transaction.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transaction == null)
            {
                return Problem("Entity set 'MvcBudgetContext.Transaction'  is null.");
            }
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction != null)
            {
                _context.Transaction.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
          return (_context.Transaction?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
