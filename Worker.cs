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
        var performanceReportService = scope.ServiceProvider.GetRequiredService<IPerformanceReportService>();
        
        _logger.LogInformation("Migrate Database");
        await constraintContext.Database.MigrateAsync(cancellationToken: stoppingToken);
        await nonConstraintContext.Database.MigrateAsync(cancellationToken: stoppingToken);

        _logger.LogInformation("Truncate Db");
        await dataGenerateService.TruncateDatabaseAsync();
        
        _logger.LogInformation("Insert customers");
        await dataGenerateService.FakeCustomerAsync();
        
        _logger.LogInformation("Insert product categories");
        await dataGenerateService.FakeProductCategoryAsync();
        
        _logger.LogInformation("Insert products");
        await dataGenerateService.FakeProductAsync();
        
        _logger.LogInformation("Insert orders");
        await dataGenerateService.FakeOrderAsync();
        
        _logger.LogInformation("Insert order details");
        await dataGenerateService.FakeOrderDetailAsync();
        
        _logger.LogInformation("Start calculate performance");
        var report = await performanceReportService.RevenueLastMonthAsync();
        _logger.LogInformation("Content: {Content}", report.Content);
        foreach (var constraintTime in report.ConstraintTimes)
        {
            _logger.LogInformation("Constraint: {Content}", constraintTime);
        }
        foreach (var nonConstraintTime in report.NonConstraintTimes)
        {
            _logger.LogInformation("Non Constraint: {Content}", nonConstraintTime);
        }
        
        report = await performanceReportService.RevenueInYearAsync();
        _logger.LogInformation("Content: {Content}", report.Content);
        foreach (var constraintTime in report.ConstraintTimes)
        {
            _logger.LogInformation("Constraint: {Content}", constraintTime);
        }
        foreach (var nonConstraintTime in report.NonConstraintTimes)
        {
            _logger.LogInformation("Non Constraint: {Content}", nonConstraintTime);
        }
        
        Environment.Exit(0);
    }
}