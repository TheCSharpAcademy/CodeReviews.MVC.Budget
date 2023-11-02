namespace MVCBudget.Forser.Repositories
{
    public class UserWalletRepository : GenericRepository<UserWallet>, IUserWalletRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserWalletRepository> _logger;

        public UserWalletRepository(AppDbContext appDbContext, ILogger<UserWalletRepository> logger) : base(appDbContext)
        {
            _context = appDbContext;
            _logger = logger;
        }
        public async Task<List<UserWallet>> GetUserWalletsAsync()
        {
            try
            {
                _logger.LogInformation($"{nameof(UserWalletRepository)} - Calling GetUserWalletsAsync()");
                var userWallets = await _context.Wallets
                    .ToListAsync();

                return userWallets;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }
    }
}