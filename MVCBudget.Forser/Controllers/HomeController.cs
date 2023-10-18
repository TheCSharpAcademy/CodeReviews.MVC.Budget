using MVCBudget.Forser.Models.ViewModels;
using System.Diagnostics;

namespace MVCBudget.Forser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly ILogger<HomeController> _logger;
        private IUserWalletRepository UserWalletRepository { get; }
        private ICategoryRepository CategoryRepository { get; }
        private ITransactionRepository TransactionRepository { get; }

        public HomeController(IDiagnosticContext diagnosticContext, ILogger<HomeController> logger,
            IUserWalletRepository userWalletRepository, ICategoryRepository categoryRepository, ITransactionRepository transactionRepository)
        {
            _diagnosticContext = diagnosticContext ??
                throw new ArgumentNullException(nameof(diagnosticContext));
            _logger = logger;
            _logger.LogInformation("HomeController called");
            UserWalletRepository = userWalletRepository;
            CategoryRepository = categoryRepository;
            TransactionRepository = transactionRepository;
        }

        public async Task<IActionResult> Index()
        {
            _diagnosticContext.Set("CatalogLoadTime", 1423);
            _logger.LogInformation($"{nameof(Index)} Starting.");

            _logger.LogInformation($"{nameof(UserWalletRepository)} is loaded");
            var wallets = await UserWalletRepository.GetUserWalletsAsync();
            _logger.LogInformation($"{nameof(CategoryRepository)}");
            var categories = await CategoryRepository.GetAllCategoriesAsync();

            var viewModel = new MainViewModel
            {
                UserWallets = wallets,
                Categories = categories
            };

            _logger.LogInformation($"{wallets.Select(s => s.Name).FirstOrDefault()} wallet is loaded");
            return View(viewModel);
        }

        public async Task<IActionResult> CreateTransaction()
        {
            return View();
        }

        [HttpPost]
        [ActionName("CreateTransaction")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TransactionRepository.CreateAsync(transaction);
                    await TransactionRepository.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Couldn't create the new Transaction: {transaction} with error {ex.Message}");
                    return View(transaction);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditTransaction(int? id)
        {
            var transaction = await TransactionRepository.GetTransactionById(id);

            if (id == null || transaction == null)
            {
                _logger.LogInformation($"Transaction with ID: {id} wasn't found.");
                return NotFound();
            }

            return View(transaction);
        }

        [HttpPost]
        [ActionName("EditTransaction")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransaction(Transaction transaction)
        {
            var exisitingTransaction = await TransactionRepository.GetTransactionById(transaction.Id);

            if (exisitingTransaction == null || transaction == null)
            {
                _logger.LogInformation($"Couldn't find the Transaction Requested : {transaction.Id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation($"Updated the Transaction with ID: {transaction.Id}");
                    exisitingTransaction.CategoryId = transaction.CategoryId;
                    exisitingTransaction.TransferredAmount = transaction.TransferredAmount;
                    exisitingTransaction.Name = transaction.Name;
                    exisitingTransaction.Description = transaction.Description;

                    await TransactionRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Databsae issue : {ex.Message} while trying to update Transaction {transaction.Id}");
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteTransaction(int? id)
        {
            var transaction = await TransactionRepository.GetTransactionById(id);

            if (id == null || transaction == null)
            {
                _logger.LogError($"Couldn't find Transaction ID : {id} while trying to delete it.");
                return NotFound();
            }

            return View(transaction);
        }

        [HttpPost]
        [ActionName("DeleteTransaction")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                _logger.LogError($"No transaction was submitted");
                return View("Error");
            }

            try
            {
                var removed = TransactionRepository.DeleteTransaction(transaction);
                if (removed)
                {
                    await TransactionRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to remove Transaction with ID: {transaction.Id}");
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ActionName("SaveCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category newCategory = new Category();
                    newCategory.Name = category.Name;

                    _logger.LogInformation($"Saving a new category {newCategory.Name}");
                    await CategoryRepository.CreateAsync(newCategory);
                    await CategoryRepository.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Couldn't Save Model: {category} with error {ex.Message}");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(MainViewModel mainViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var category = await CategoryRepository.GetCategoryByIdAsync(mainViewModel.Categories[0].Id);

                    if (category == null)
                    {
                        _logger.LogError($"Category with ID: {mainViewModel.Categories[0].Id} couldn't be found.");
                        return Problem("Couldn't find Category");
                    }
                    
                    if (category != null)
                    {
                        _logger.LogInformation($"Deleting category: {mainViewModel.Categories[0].Name}");
                        await CategoryRepository.DeleteAsync(category.Id);
                        await CategoryRepository.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Couldn't delete Category: {mainViewModel.Categories[0].Name} due to error : {ex.Message} ");
                }
            }

            return View(nameof(Index));
        }

        [HttpPost]
        [ActionName("UpdateCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(MainViewModel mainViewModel)
        {
            var existingCategory = await CategoryRepository.GetCategoryByIdAsync(mainViewModel.Categories[0].Id);

            if (existingCategory == null)
            {
                _logger.LogError($"Failed to find Category with ID: {mainViewModel.Categories[0].Id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingCategory.Name = mainViewModel.Categories[0].Name;

                    await CategoryRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Couldn't update Category, Error : {ex.Message}");
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError($"RequestedID : {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}