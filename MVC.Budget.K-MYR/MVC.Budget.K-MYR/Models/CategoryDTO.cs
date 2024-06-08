namespace MVC.Budget.K_MYR.Models
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public int CategoryType { get; set; }
        public string? Name { get; set; }
        public decimal Budget { get; set; }
        public decimal Total { get; set; }
        public decimal HappyTotal { get; set; }
        public decimal NecessaryTotal { get; set; }
        public BudgetLimit? BudgetLimit { get; internal set; }
    }
}
