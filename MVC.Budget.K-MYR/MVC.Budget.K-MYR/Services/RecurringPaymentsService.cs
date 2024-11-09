using MVC.Budget.K_MYR.Data;

namespace MVC.Budget.K_MYR.Services;

public class RecurringPaymentsService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IUnitOfWork _unitOfWork;

    public RecurringPaymentsService(IServiceScopeFactory scopeFactory, IUnitOfWork unitOfWork)
    {
        _scopeFactory = scopeFactory;
        _unitOfWork = unitOfWork;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            // Retrieve recurring transactions due for processing
            // Process recurring transactions

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Run task every day
        }
    }
}