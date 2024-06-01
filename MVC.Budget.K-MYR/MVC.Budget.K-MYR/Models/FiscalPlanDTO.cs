
namespace MVC.Budget.K_MYR.Models
{
    public class FiscalPlanDTO
    {
        public int Id { get; set; }
        public decimal Overspending { get; set; }
        public decimal IncomeBudget { get; set; }
        public decimal IncomeTotal { get; set; }
        public decimal ExpensesNecessaryTotal { get; set; }
        public decimal ExpensesHappyTotal { get; set; }
        public decimal ExpensesBudget { get; set; }
        public decimal ExpensesTotal { get; set; }
        public IEnumerable<CategoryDTO> IncomeCategories { get; set; }
        public IEnumerable<CategoryDTO> ExpenseCategories { get; set; }
    }
}
