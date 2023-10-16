using System.ComponentModel;
using System.Diagnostics;

namespace MVCBudget.Forser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly ILogger<HomeController> _logger;
        private IUserWalletRepository UserWalletRepository { get; }
        private const string SessionGuid = "_Guid";

        public HomeController(IDiagnosticContext diagnosticContext, ILogger<HomeController> logger, IUserWalletRepository userWalletRepository)
        {
            _diagnosticContext = diagnosticContext ??
                throw new ArgumentNullException(nameof(diagnosticContext));
            _logger = logger;
            _logger.LogInformation("HomeController called");
            UserWalletRepository = userWalletRepository;
        }
        public async Task<IActionResult> Index()
        {
            _diagnosticContext.Set("CatalogLoadTime", 1423);
            _logger.LogInformation($"{nameof(Index)} Starting.");
            _logger.LogInformation("Calling Verify User Guid to verify a Guid if existing");

            if (HttpContext.Session.GetString(SessionGuid) != null)
            {
                Guid userGuid = new Guid(HttpContext.Session.GetString(SessionGuid));

                var validUser = await UserWalletRepository.VerifyUserGuidAsync(userGuid);
                if (validUser)
                {
                    _logger.LogInformation($"{nameof(UserWalletRepository)} is loaded");
                    var wallets = await UserWalletRepository.GetUserWalletsAsync();

                    _logger.LogInformation($"{wallets.Select(s => s.Name).FirstOrDefault()} wallet is loaded");
                    return View(wallets);
                }
            }

            return RedirectToAction("Register");
        }
        public async Task<IActionResult> Register()
        {
            var registerWallet = new RegisterUserWallet();

            return View(registerWallet);
        }
        [HttpPost]
        [ActionName("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserWallet registerWallet)
        {
            if (ModelState.IsValid)
            {
                registerWallet.UserGuid = Guid.NewGuid();

                HttpContext.Session.SetString(SessionGuid, registerWallet.UserGuid.ToString());

                return View(registerWallet);
            }
            return View(registerWallet);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError($"RequestedID : {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}