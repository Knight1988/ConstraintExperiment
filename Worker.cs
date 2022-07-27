using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Models;

namespace ConstraintExperiment;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IDataGenerateService _dataGenerateService;
    private readonly IPerformanceReportService _performanceReportService;

    public Worker(ILogger<Worker> logger, IDataGenerateService dataGenerateService, 
        IPerformanceReportService performanceReportService)
    {
        _logger = logger;
        _dataGenerateService = dataGenerateService;
        _performanceReportService = performanceReportService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Migrate Database");
        await _dataGenerateService.MigrateDatabaseAsync();
        
        _logger.LogInformation("Truncate Db");
        await _dataGenerateService.TruncateDatabaseAsync();
        
        _logger.LogInformation("Insert customers");
        await _dataGenerateService.FakeCustomerAsync();
        
        _logger.LogInformation("Insert product categories");
        await _dataGenerateService.FakeProductCategoryAsync();
        
        _logger.LogInformation("Insert products");
        await _dataGenerateService.FakeProductAsync();
        
        _logger.LogInformation("Insert orders");
        await _dataGenerateService.FakeOrderAsync();
        
        _logger.LogInformation("Insert order details");
        await _dataGenerateService.FakeOrderDetailAsync();
        
        _logger.LogInformation("Start calculate performance");
        var reports = new List<PerformanceReport>();
        var report = await _performanceReportService.SearchProductAsync();
        reports.Add(report);
        report = await _performanceReportService.RevenueLastMonthAsync();
        reports.Add(report);
        report = await _performanceReportService.RevenueInYearAsync();
        reports.Add(report);
        report = await _performanceReportService.BestSellerProductInYearAsync();
        reports.Add(report);
        report = await _performanceReportService.TopCustomerInYearAsync();
        reports.Add(report);
        
        _logger.LogInformation("Creating reports");
        var filePath = await _performanceReportService.WriteToFileAsync(reports);
        
        _logger.LogInformation("Created report {FilePath}", filePath);
        
        Environment.Exit(0);
    }
}