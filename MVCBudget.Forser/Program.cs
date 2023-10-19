// TODO: Create a Search for Transactions by name
// TODO: Filter Function for Transactions per Category or/and Date
// TODO: Modals for Insert, Delete, Update Transactions/Categories
//
// TODO: Create a Modal that opens up if a User visits for the first time.
// TODO: Show a modal on first time visit to create a Wallet for Visitor (Create unique ID with Guid on Save and assign a Session/Cookie to visitor)
//
// TODO: Allow Export of Transactions to Excel Document with EPPlus

using MVCBudget.Forser.Middleware;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

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