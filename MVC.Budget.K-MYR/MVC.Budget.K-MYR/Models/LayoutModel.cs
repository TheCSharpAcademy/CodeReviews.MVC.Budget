using System.Globalization;

namespace MVC.Budget.K_MYR.Models;

public class LayoutModel(CultureInfo culture)
{
    public CultureInfo Culture { get; set; } = culture;
}


public class LayoutModel<T>(T model, CultureInfo culture) : LayoutModel(culture)
{
    public T PageModel { get; set; } = model;
}