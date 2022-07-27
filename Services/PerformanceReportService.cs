using ConstraintExperiment.Commons;
using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models;

namespace ConstraintExperiment.Services;

public class PerformanceReportService : IPerformanceReportService
{
    private readonly IConfiguration _configuration;
    private readonly IReportMonthRepo _reportMonthRepo;
    private readonly IReportMonth2Repo _reportMonth2Repo;
    private readonly IReportYearlyRepo _reportYearlyRepo;
    private readonly IReportYearly2Repo _reportYearly2Repo;
    private readonly IProductRepo _productRepo;
    private readonly IProduct2Repo _product2Repo;

    public PerformanceReportService(IConfiguration configuration,
        IReportMonthRepo reportMonthRepo, IReportMonth2Repo reportMonth2Repo,
        IReportYearlyRepo reportYearlyRepo, IReportYearly2Repo reportYearly2Repo,
        IProductRepo productRepo, IProduct2Repo product2Repo)
    {
        _configuration = configuration;
        _reportMonthRepo = reportMonthRepo;
        _reportMonth2Repo = reportMonth2Repo;
        _reportYearlyRepo = reportYearlyRepo;
        _reportYearly2Repo = reportYearly2Repo;
        _productRepo = productRepo;
        _product2Repo = product2Repo;
    }

    public async Task<string> WriteToFileAsync(IEnumerable<PerformanceReport> reports)
    {
        const string fileName = "PerformanceReport.md";
        await using StreamWriter file = new(fileName);
        await file.WriteLineAsync("## Performance Test Report");
        await file.WriteLineAsync(string.Empty);
        await file.WriteLineAsync($"Customer: {_configuration.GetValue<int>("Fakes:CustomerCount")}");
        await file.WriteLineAsync($"Product: {_configuration.GetValue<int>("Fakes:ProductCount")}");
        await file.WriteLineAsync($"Order: {_configuration.GetValue<int>("Fakes:OrderCount")}");
        await file.WriteLineAsync(string.Empty);

        foreach (var report in reports)
        {
            var totalConstraint = 0d;
            var totalNonConstraint = 0d;
            await file.WriteLineAsync("|Test case|Constraint|Non Constraint|");
            await file.WriteLineAsync("|--|--|--|");
            var repeatTime = _configuration.GetValue<int>("TestRepeatTimes");
            for (var i = 0; i < repeatTime; i++)
            {
                totalConstraint += report.ConstraintTimes[i];
                totalNonConstraint += report.NonConstraintTimes[i];
                await file.WriteLineAsync(
                    $"|{report.Content}|{report.ConstraintTimes[i]}|{report.NonConstraintTimes[i]}|");
            }

            var avgConstraint = totalConstraint / repeatTime;
            var avgNonConstraint = totalNonConstraint / repeatTime;
            await file.WriteLineAsync($"|Avg|{avgConstraint}|{avgNonConstraint}|");
            await file.WriteLineAsync(string.Empty);
        }

        await file.FlushAsync();
        return Path.GetFullPath(fileName);
    }

    private async Task<PerformanceReport> PerformanceTest<TNonConstraint, TConstraint>(string content, 
        Func<Task<TNonConstraint>> nonConstraintFunc,
        Func<Task<TConstraint>> constraintFunc)
    {
        var report = new PerformanceReport
        {
            Content = content,
            ConstraintTimes = new List<long>(),
            NonConstraintTimes = new List<long>()
        };

        var repeatTimes = _configuration.GetValue<int>("TestRepeatTimes");

        for (var i = 0; i <= repeatTimes; i++)
        {
            var elapsedTime = await nonConstraintFunc().GetElapsedTimeAsync();
            // skip first result
            if (i != 0) report.NonConstraintTimes.Add(elapsedTime);
        }

        for (var i = 0; i <= repeatTimes; i++)
        {
            var elapsedTime = await constraintFunc().GetElapsedTimeAsync();
            // skip first result
            if (i != 0) report.ConstraintTimes.Add(elapsedTime);
        }

        return report;
    }

    public async Task<PerformanceReport> RevenueLastMonthAsync()
    {
        return await PerformanceTest("Get revenue last month", 
            () => _reportMonthRepo.RevenueMonthlyAsync(3),
            () => _reportMonth2Repo.RevenueMonthlyAsync(3));
    }

    public async Task<PerformanceReport> RevenueInYearAsync()
    {
        var thisYear = DateTime.Now.Year;
        return await PerformanceTest("Get revenue this year", 
            () => _reportYearlyRepo.RevenueInYearAsync(thisYear),
            () => _reportYearly2Repo.RevenueInYearAsync(thisYear));
    }

    public async Task<PerformanceReport> BestSellerProductInYearAsync()
    {
        var thisYear = DateTime.Now.Year;
        return await PerformanceTest("Best seller product this year", 
            () => _reportYearlyRepo.BestSellerInYearAsync(thisYear, 10),
            () => _reportYearly2Repo.BestSellerInYearAsync(thisYear, 10));
    }

    public async Task<PerformanceReport> TopCustomerInYearAsync()
    {
        var thisYear = DateTime.Now.Year;
        return await PerformanceTest("Top customer this year", 
            () => _reportYearlyRepo.TopCustomerInYearAsync(thisYear, 10),
            () => _reportYearly2Repo.TopCustomerInYearAsync(thisYear, 10));
    }

    public async Task<PerformanceReport> SearchProductAsync()
    {
        return await PerformanceTest("Search Product", 
            () => _productRepo.SearchAsync("Chicken", 0, 20),
            () => _product2Repo.SearchAsync("Chicken", 0, 20));
    }
}