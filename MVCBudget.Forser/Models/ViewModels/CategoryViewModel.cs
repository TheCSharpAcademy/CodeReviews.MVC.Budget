namespace MVCBudget.Forser.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter a category name")]
        [RegularExpression(@"^[a-zA-Z\s.\-']{2,}$", ErrorMessage = "Name contains invalid characters.")]
        public string Name { get; set; } = string.Empty;
    }
}