namespace MVCBudget.Forser.Repositories
{
    public interface IUserWalletRepository : IGenericRepository<UserWallet>
    {
        Task<bool> VerifyUserGuidAsync(Guid userGuid);
        Task<List<UserWallet>> GetUserWalletsAsync();
    }
}