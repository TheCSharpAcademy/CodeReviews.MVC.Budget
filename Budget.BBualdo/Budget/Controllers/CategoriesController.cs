using Budget.Models;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget.Controllers;

public class CategoriesController(BudgetDbContext context) : Controller
{
    private readonly BudgetDbContext _context = context;

    // GET: Category
    public async Task<IActionResult> Index()
    {
        var transactions = await _context.Transactions.Include(t => t.Category).OrderBy(t => t.Date).ToListAsync();
        var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();

        var viewModel = new BudgetVM
        {
            Transactions = transactions,
            Categories = categories,
            TransactionViewModel = new TransactionsVM(categories),
            CategoryViewModel = new CategoriesVM()
        };

        return View(viewModel);
    }

    // GET: Category/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var category = await _context.Categories
            .FirstOrDefaultAsync(m => m.Id == id);

        if (category == null) return NotFound();

        return Json(category);
    }

    // POST: Category/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BudgetVM budgetViewModel)
    {
        var newCategory = new Category
        {
            Id = budgetViewModel.CategoryViewModel.Id,
            Name = budgetViewModel.CategoryViewModel.Name
        };

        if (newCategory.Id > 0)
            _context.Update(newCategory);
        else
            _context.Add(newCategory);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // POST: Category/Delete/5
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category != null) _context.Categories.Remove(category);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> DuplicateCategoryName([Bind(Prefix = "CategoryViewModel.Name")] string name)
    {
        var categories = await _context.Categories.ToListAsync();

        if (categories.Any(c => c.Name.ToLower() == name.ToLower()))
            return Json("Duplicate Category");

        return Json(true);
    }
}