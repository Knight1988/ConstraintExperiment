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
        using var scope = _scopeFactory.CreateScope();
        var constraintContext = scope.ServiceProvider.GetRequiredService<ConstraintContext>();
        var nonConstraintContext = scope.ServiceProvider.GetRequiredService<NonConstraintContext>();
        var dataGenerateService = scope.ServiceProvider.GetRequiredService<IDataGenerateService>();
        
        _logger.LogInformation("Migrate Database");
        await constraintContext.Database.MigrateAsync(cancellationToken: stoppingToken);
        await nonConstraintContext.Database.MigrateAsync(cancellationToken: stoppingToken);

        _logger.LogInformation("Truncate Db");
        await dataGenerateService.TruncateDatabaseAsync();
        
        _logger.LogInformation("Insert customers");
        await dataGenerateService.FakeCustomerAsync();
        
        _logger.LogInformation("Insert products");
        await dataGenerateService.FakeProductAsync();
        
        Environment.Exit(0);
    }
}