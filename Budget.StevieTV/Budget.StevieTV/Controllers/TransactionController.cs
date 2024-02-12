using Budget.StevieTV.Database;
using Budget.StevieTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Budget.StevieTV.Controllers
{
    public class TransactionController : Controller
    {
        private readonly BudgetContext _context;

        public TransactionController(BudgetContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            
            var transactions = await _context.Transactions.Include(t => t.Category).OrderBy(t => t.Date).ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            var viewModel = new BudgetViewModel
            {
                Transactions = transactions,
                Categories = categories,
                TransactionViewModel = new TransactionViewModel(categories),
                CategoryViewModel = new CategoryViewModel()
            };
            
            return View(viewModel);
        }
        
        // GET: Transaction/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }
      
            return Json(transaction);
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BudgetViewModel budgetViewModel)
        {
            var newTransaction = new Transaction
            {
                Id = budgetViewModel.TransactionViewModel.Id,
                TransactionType = budgetViewModel.TransactionViewModel.TransactionType,
                Date = budgetViewModel.TransactionViewModel.Date,
                Description = budgetViewModel.TransactionViewModel.Description,
                Amount = budgetViewModel.TransactionViewModel.Amount,
                CategoryId = budgetViewModel.TransactionViewModel.CategoryId
            };

            if (newTransaction.Id > 0)
            {
                _context.Update(newTransaction);
            }
            else
            {
                _context.Add(newTransaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TransactionType,Date,Description,Amount,CategoryId")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
