using MVC.Budget.K_MYR.Models;
using System.Reflection;

namespace MVC.Budget.K_MYR.Extensions;

public static class OrderingHelpers
{
    private static readonly HashSet<string> _allowedProperties = new(StringComparer.OrdinalIgnoreCase)
    {
        nameof(Transaction.DateTime),
        nameof(Transaction.Title),
        nameof(Transaction.Amount),
        string.Join(".", nameof(Category), nameof(Category.Name))
    };

    public static bool IsAllowedProperty(string propertyName)
    {
        return _allowedProperties.Contains(propertyName);
    }

    public static PropertyInfo? GetProperty<T>(string propertyName)
    {
        string[] properties = propertyName.Split('.');
        PropertyInfo? propertyInfo = null;
        Type type = typeof(T);

        foreach (string prop in properties)
        {
            propertyInfo = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                return null;
            }

            type = propertyInfo.PropertyType;
        }

        return propertyInfo;
    }
}
