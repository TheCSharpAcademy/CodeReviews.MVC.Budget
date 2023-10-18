namespace MVCBudget.Forser.Models.ViewModels
{
    public class MainViewModel
    {
        public List<UserWallet> UserWallets { get; set; } = new List<UserWallet>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}