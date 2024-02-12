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
    }
}
