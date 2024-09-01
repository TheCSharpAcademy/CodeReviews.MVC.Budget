using Microsoft.AspNetCore.Mvc;

namespace Budget.Models;

public class CategoriesVM
{
    public int Id { get; set; }
    [Remote("DuplicateCategoryName", "Categories")]
    public string Name { get; set; }
}