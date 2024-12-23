using Budget.jollejonas.Data;
using Budget.jollejonas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Budget.jollejonas.Controllers;

public class TransactionsController : Controller
{
    private readonly BudgetjollejonasContext _context;

    public TransactionsController(BudgetjollejonasContext context)
    {
        _context = context;
    }

    // GET: Categories
    public async Task<IActionResult> Index(string searchString, int? year, int? month, int? categoryId)
    {
        var transactionsQuery = _context.Transaction
            .Include(t => t.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            transactionsQuery = transactionsQuery.Where(t => t.Name.Contains(searchString));
        }
        if (year.HasValue)
        {
            transactionsQuery = transactionsQuery.Where(t => t.Date.Year == year.Value);
        }
        if (month.HasValue)
        {
            transactionsQuery = transactionsQuery.Where(t => t.Date.Month == month.Value);
        }
        if (categoryId.HasValue)
        {
            transactionsQuery = transactionsQuery.Where(t => t.CategoryId == categoryId.Value);
        }

        var transactions = await transactionsQuery.ToListAsync();

        var categories = await _context.Category.ToListAsync();

        var viewModel = new TransactionCategoryViewModel
        {
            Transaction = transactions,
            Categories = categories,
            CategoriesList = new SelectList(categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList(), "Value", "Text"),
            YearList = new SelectList(Enumerable.Range(DateTime.Now.Year - 10, 21)
                .Select(y => new SelectListItem { Value = y.ToString(), Text = y.ToString() }).ToList(), "Value", "Text"),
            MonthList = new SelectList(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
                .Where(m => !string.IsNullOrEmpty(m))
                .Select((m, index) => new SelectListItem { Value = (index + 1).ToString(), Text = m }).ToList(), "Value", "Text"),
            SearchString = searchString,
            CategoryId = categoryId ?? 0,
            Year = year ?? DateTime.Now.Year,
            Month = month ?? DateTime.Now.Month
        };

        return View(viewModel);
    }

    // GET: Transactions/Details/5
    public async Task<ActionResult<Transaction>> Details(int? id)
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

        ViewBag.Categories = _context.Category.ToList();
        return PartialView("_EditModal", transaction);
    }

    // POST: Transactions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Amount,Date,CategoryId")] Transaction transaction)
    {
        if (transaction.CategoryId != 0)
        {
            ModelState.Remove("Category");
        }
        if (id != transaction.Id)
        {
            return BadRequest();
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

        return PartialView("_EditModal", transaction);
    }

    // GET: Transactions/Create
    public IActionResult Create()
    {
        ViewBag.Categories = _context.Category.ToList();
        return PartialView("_CreateModal");
    }

    // POST: Transactions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Amount,Date,CategoryId")] Transaction transaction)
    {
        if (transaction.CategoryId != 0)
        {
            ModelState.Remove("Category");
        }
        if (!ModelState.IsValid)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
            }
        }
        if (ModelState.IsValid)
        {
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        var transactions = await _context.Transaction.ToListAsync();
        return View("Index", transactions);
    }

    // POST: Transactions/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var transaction = await _context.Transaction.FindAsync(id);
        if (transaction != null)
        {
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool TransactionExists(int id)
    {
        return _context.Transaction.Any(e => e.Id == id);
    }
}

