using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Models;
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
        var reports = new List<PerformanceReport>();
        var report = await performanceReportService.RevenueLastMonthAsync();
        reports.Add(report);
        report = await performanceReportService.RevenueInYearAsync();
        reports.Add(report);
        report = await performanceReportService.BestSellerProductInYearAsync();
        reports.Add(report);
        
        _logger.LogInformation("Creating reports");
        var filePath = await performanceReportService.WriteToFileAsync(reports);
        
        _logger.LogInformation("Created report {FilePath}", filePath);
        
        Environment.Exit(0);
    }
}