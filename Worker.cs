using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Migrate Database");
        using var scope = _scopeFactory.CreateScope();
        var constraintContext = scope.ServiceProvider.GetRequiredService<ConstraintContext>();
        var nonConstraintContext = scope.ServiceProvider.GetRequiredService<NonConstraintContext>();
        var dataGenerateService = scope.ServiceProvider.GetRequiredService<IDataGenerateService>();
        
        await constraintContext.Database.MigrateAsync(cancellationToken: stoppingToken);
        await nonConstraintContext.Database.MigrateAsync(cancellationToken: stoppingToken);

        await dataGenerateService.TruncateDatabaseAsync();
        await dataGenerateService.FakeCustomerAsync();
        
        Environment.Exit(0);
    }
}