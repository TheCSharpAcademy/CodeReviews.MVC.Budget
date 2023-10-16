namespace MVCBudget.Forser.Models
{
    public class RegisterUserWallet
    {
        public Guid UserGuid {  get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}