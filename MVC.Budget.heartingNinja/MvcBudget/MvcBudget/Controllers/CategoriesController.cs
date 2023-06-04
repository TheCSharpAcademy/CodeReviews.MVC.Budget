using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcBudget.Data;
using MvcBudget.Models;

namespace MvcBudget.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly MvcBudgetContext _context;

        public CategoriesController(MvcBudgetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Category != null ?
                View(await _context.Category.ToListAsync()) :
                Problem("Entity set 'MvcBudgetContext.Category' is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int selectedCategoryId)
        {
            if (_context.Category == null)
            {
                return Problem("Entity set 'MvcBudgetContext.Category' is null.");
            }

            var category = await _context.Category.FindAsync(selectedCategoryId);
            if (category == null)
            {
                return NotFound();
            }
           
            var transactionsWithCategory = _context.Transaction.Where(t => t.CategoryId == selectedCategoryId);
            _context.Transaction.RemoveRange(transactionsWithCategory);

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Transactions");
        }

        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

