using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace kmakai.Budget.Models.ViewModels;

public class AddCategoryViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
}
