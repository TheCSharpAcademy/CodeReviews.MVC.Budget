using Budget.Models;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget.Controllers;

public class TransactionsController(BudgetDbContext context) : Controller
{
    // GET: Transaction
    public async Task<IActionResult> Index(string searchString, string categoryId, string startDate, string endDate)
    {
        var transactions = await context.Transactions.Include(t => t.Category).OrderBy(t => t.Date).ToListAsync();
        var categories = await context.Categories.ToListAsync();

        if (!string.IsNullOrEmpty(searchString))
            transactions = transactions
                .Where(t => t.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

        if (!string.IsNullOrEmpty(categoryId))
            transactions = transactions.Where(t => t.CategoryId.ToString() == categoryId).ToList();

        if (!string.IsNullOrEmpty(startDate))
        {
            var inputDate = DateTime.Parse(startDate);
            transactions = transactions.Where(t => t.Date >= inputDate).ToList();
        }

        if (!string.IsNullOrEmpty(endDate))
        {
            var inputDate = DateTime.Parse(endDate);
            transactions = transactions.Where(t => t.Date <= inputDate).ToList();
        }

        var viewModel = new BudgetVM
        {
            Transactions = transactions,
            Categories = categories,
            TransactionViewModel = new TransactionsVM(categories),
            CategoryViewModel = new CategoriesVM()
        };


        return View(viewModel);
    }

    // GET: Transaction/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var transaction = await context.Transactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (transaction == null) return NotFound();

        return Json(transaction);
    }

    // POST: Transaction/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BudgetVM budgetViewModel)
    {
        var newTransaction = new Transaction
        {
            Id = budgetViewModel.TransactionViewModel.Id,
            Date = budgetViewModel.TransactionViewModel.Date,
            Title = budgetViewModel.TransactionViewModel.Title,
            Amount = budgetViewModel.TransactionViewModel.Amount,
            CategoryId = budgetViewModel.TransactionViewModel.CategoryId
        };

        if (newTransaction.Id > 0)
            context.Update(newTransaction);
        else
            context.Add(newTransaction);

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: Transaction/Delete/5
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var transaction = await context.Transactions.FindAsync(id);
        if (transaction != null) context.Transactions.Remove(transaction);

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}