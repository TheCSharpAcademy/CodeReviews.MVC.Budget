using Microsoft.AspNetCore.Mvc;

namespace Budget.StevieTV.Models;

public class CategoryViewModel
{
    public int Id { get; set; }
    [Remote("DuplicateCategoryName", "Category")]
    public string Name { get; set; }
    
}