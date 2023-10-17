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

        public HomeController(IDiagnosticContext diagnosticContext, ILogger<HomeController> logger, 
            IUserWalletRepository userWalletRepository, ICategoryRepository categoryRepository)
        {
            _diagnosticContext = diagnosticContext ??
                throw new ArgumentNullException(nameof(diagnosticContext));
            _logger = logger;
            _logger.LogInformation("HomeController called");
            UserWalletRepository = userWalletRepository;
            CategoryRepository = categoryRepository;
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
        [HttpPost]
        [ActionName("SaveCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCategory(MainViewModel mainViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category newCategory = new Category();
                    newCategory.Name = mainViewModel.Categories[0].Name;

                    _logger.LogInformation($"Saving a new category {newCategory.Name}");
                    await CategoryRepository.CreateAsync(newCategory);
                    await CategoryRepository.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Couldn't Save Model: {mainViewModel} with error {ex.Message}");
                }
            }

            return View("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError($"RequestedID : {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}