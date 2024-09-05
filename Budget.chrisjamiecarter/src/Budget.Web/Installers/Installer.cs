using Budget.Infrastructure.Installers;

namespace Budget.Web.Installers;

/// <summary>
/// Registers dependencies and adds any required middleware for the Presentation layer.
/// </summary>
public static class Installer
{
    public static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddControllersWithViews();

        return services;
    }

    public static WebApplication AddMiddleware(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Transactions}/{action=Index}/{id?}");

        return app;
    }

    public static WebApplication SetUpDatabase(this WebApplication app)
    {
        // Performs any database migrations and seeds the database.
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        services.SeedDatabase();

        return app;
    }
}
