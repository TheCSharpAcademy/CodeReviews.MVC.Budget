using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Data;
public static class SeedData
{
    public static void InitializeDatabase(DatabaseContext context)
    {
        if(!context.Groups.Any())
        {
            context.AddRange(new[]
            {
                new Group(),
                new Group(),
                new Group(),
            });
            context.SaveChanges();
        }
    }
}
