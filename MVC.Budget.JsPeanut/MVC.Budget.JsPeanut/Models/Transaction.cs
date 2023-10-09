namespace MVC.Budget.JsPeanut.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
