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

        public async Task<bool> VerifyUserGuidAsync(Guid userGuid)
        {
            _logger.LogInformation($"Verifying User GUID against Database");
            try
            {
                var userValid = await _context.Wallets.Select(s => s.UserId == userGuid).FirstOrDefaultAsync();
                if (userValid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - When trying to verify GUID");
                throw new Exception($"{ex.Message}", ex);
            }
        }
        public async Task<List<UserWallet>> GetUserWalletsAsync()
        {
            try
            {
                _logger.LogInformation($"{nameof(UserWalletRepository)} - Calling GetUserWalletsAsync()");
                var userWallets = await _context.Wallets
                    .Include(uw => uw.Transactions)
                    .ThenInclude(t => t.Category)
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