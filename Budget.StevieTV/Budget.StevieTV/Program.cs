using System.Globalization;
using Budget.StevieTV.Database;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BudgetContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString") ?? throw new InvalidOperationException("Connection string not found")));
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<BudgetContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var cultureInfo = new CultureInfo("en-US")
{
    NumberFormat =
    {
        CurrencyDecimalSeparator = ".",
        CurrencyGroupSeparator = ",",
        CurrencySymbol = "€"
    }
};

var supportedCultures = new CultureInfo[]
{
    cultureInfo
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultureInfo, cultureInfo),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transaction}/{action=Index}/{id?}");

app.Run();