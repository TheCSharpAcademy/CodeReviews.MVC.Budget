using Budget.Application.Services;
using Budget.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Web.Controllers;

/// <summary>
/// Manages the Transaction-related actions for the Presentation layer.
/// This controller handles the CRUD operations and also provides filtering and sorting functionalities.
/// </summary>
public class TransactionsController : Controller
{
    #region Fields

    private readonly ICategoryService _categoryService;
    private readonly ITransactionService _transactionService;

    #endregion
    #region Constructors

    public TransactionsController(ICategoryService categoryService, ITransactionService transactionService)
    {
        _categoryService = categoryService;
        _transactionService = transactionService;
    }

    #endregion
    #region Methods

    // GET: Transactions
    public async Task<IActionResult> Index(string searchName, string searchStart, string searchEnd, string filterCategory)
    {
        var entities = await _transactionService.ReturnAsync(includeProperties: "Category");

        var viewModel = new TransactionsViewModel();

        if (!string.IsNullOrWhiteSpace(searchName))
        {
            viewModel.SearchName = searchName;
            entities = entities.Where(e => e.Name!.Contains(searchName, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(searchStart))
        {
            viewModel.SearchStart = searchStart;
            var startDate = DateTime.Parse(searchStart);
            entities = entities.Where(e => e.Date >= startDate);
        }

        if (!string.IsNullOrWhiteSpace(searchEnd))
        {
            viewModel.SearchEnd = searchEnd;
            var endDate = DateTime.Parse(searchEnd);
            entities = entities.Where(e => e.Date <= endDate);
        }

        if (!string.IsNullOrWhiteSpace(filterCategory))
        {
            viewModel.FilterCategory = filterCategory;
            entities = entities.Where(e => e.Category!.Id == Guid.Parse(filterCategory));
        }

        entities = entities.OrderBy(e => e.Date);

        viewModel.SetCategories(await GetCategoriesAsync());
        viewModel.Transactions = entities.Select(x => new TransactionViewModel(x));

        return View(viewModel);
    }

    // GET: Transactions/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (!id.HasValue)
        {
            return NotFound();
        }

        var entity = await _transactionService.ReturnAsync(id.Value);
        if (entity is null)
        {
            return NotFound();
        }

        var transaction = new TransactionViewModel(entity);
        return Ok(transaction);
    }

    // GET: Transactions/Create
    public async Task<IActionResult> Create()
    {
        var categories = await GetCategoriesAsync();

        var viewModel = new TransactionViewModel(categories);

        return PartialView("_CreatePartial", viewModel);
    }

    // POST: Transactions/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Date,Amount,CategoryId")] TransactionViewModel transaction)
    {
        if (ModelState.IsValid)
        {
            var category = await _categoryService.ReturnAsync(transaction.CategoryId);
            if (category is null)
            {
                return NotFound();
            }
            transaction.Id = Guid.NewGuid();
            transaction.Category = new CategoryViewModel(category);
            await _transactionService.CreateAsync(transaction.MapToDomain());
            return Json(new { success = true });
        }

        // Reset the SelectList for #Reasons...
        transaction.SetCategories(await GetCategoriesAsync());

        return PartialView("_CreatePartial", transaction);
    }

    // GET: Transactions/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (!id.HasValue)
        {
            return NotFound();
        }

        var entity = await _transactionService.ReturnAsync(id.Value);
        if (entity is null)
        {
            return NotFound();
        }

        var categories = await GetCategoriesAsync();
        var viewModel = new TransactionViewModel(entity, categories);
        return PartialView("_EditPartial", viewModel);
    }

    // POST: Transactions/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Date,Amount,CategoryId")] TransactionViewModel transaction)
    {
        if (id != transaction.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var category = await _categoryService.ReturnAsync(transaction.CategoryId);
            if (category is null)
            {
                return NotFound();
            }
            transaction.Category = new CategoryViewModel(category);
            await _transactionService.UpdateAsync(transaction.MapToDomain());
            return Json(new { success = true });
        }

        // Reset the SelectList for #Reasons...
        transaction.SetCategories(await GetCategoriesAsync());

        return PartialView("_EditPartial", transaction);
    }

    // GET: Transactions/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (!id.HasValue)
        {
            return NotFound();
        }

        var entity = await _transactionService.ReturnAsync(id.Value);
        if (entity is null)
        {
            return NotFound();
        }

        var transaction = new TransactionViewModel(entity);
        return PartialView("_DeletePartial", transaction);
    }

    // POST: Transactions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _transactionService.DeleteAsync(id);
        return Json(new { success = true });
    }

    private async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
    {
        var entities = await _categoryService.ReturnAsync();
        return entities.Select(x => new CategoryViewModel(x));
    }

    #endregion
}
