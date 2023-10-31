using Microsoft.AspNetCore.Localization;
using MVCBudget.Forser.Middleware;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var supportedCultures = new[]
{
    new CultureInfo("en-US")
};

try
{
    Log.Information($"Starting web application in {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("MSSQLConnection"));
    })
    .AddScoped<AppDbContext>()
    .AddScoped<IUserWalletRepository, UserWalletRepository>()
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<ITransactionRepository, TransactionRepository>();

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    
    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        await SeedData.Initalize(services);
    }

    app.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    });

    app.UseSerilogRequestLogging();
    app.UseMiddleware(typeof(ExceptionHandlingMiddleware));

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

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}