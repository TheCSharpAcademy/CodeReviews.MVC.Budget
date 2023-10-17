namespace MVCBudget.Forser.Repositories
{
    public interface IUserWalletRepository : IGenericRepository<UserWallet>
    {
        Task<List<UserWallet>> GetUserWalletsAsync();
    }
}