namespace MVC.Budget.JsPeanut.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
