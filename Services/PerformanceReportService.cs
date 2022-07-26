using System.Diagnostics;
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
                await file.WriteLineAsync($"|{report.Content}|{report.ConstraintTimes[i]}|{report.NonConstraintTimes[i]}|");
            }

            var avgConstraint = totalConstraint / repeatTime;
            var avgNonConstraint = totalNonConstraint / repeatTime;
            await file.WriteLineAsync($"|Avg|{avgConstraint}|{avgNonConstraint}|");
            await file.WriteLineAsync(string.Empty);
        }

        await file.FlushAsync();
        return Path.GetFullPath(fileName);
    }

    public async Task<PerformanceReport> RevenueLastMonthAsync()
    {
        var report = new PerformanceReport
        {
            Content = "Get revenue last month",
            ConstraintTimes = new List<long>(),
            NonConstraintTimes = new List<long>()
        };
        var repeatTimes = _configuration.GetValue<int>("TestRepeatTimes");

        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _reportMonthRepo.RevenueMonthlyAsync(3).GetElapsedTimeAsync();
            report.NonConstraintTimes.Add(elapsedTime);
        }
        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _reportMonth2Repo.RevenueMonthlyAsync(3).GetElapsedTimeAsync();
            report.ConstraintTimes.Add(elapsedTime);
        }

        return report;
    }

    public async Task<PerformanceReport> RevenueInYearAsync()
    {
        var report = new PerformanceReport
        {
            Content = "Get revenue this year",
            ConstraintTimes = new List<long>(),
            NonConstraintTimes = new List<long>()
        };
        var repeatTimes = _configuration.GetValue<int>("TestRepeatTimes");

        var thisYear = DateTime.Now.Year;
        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _reportYearlyRepo.RevenueInYearAsync(thisYear).GetElapsedTimeAsync();
            report.NonConstraintTimes.Add(elapsedTime);
        }
        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _reportYearly2Repo.RevenueInYearAsync(thisYear).GetElapsedTimeAsync();
            report.ConstraintTimes.Add(elapsedTime);
        }

        return report;
    }

    public async Task<PerformanceReport> BestSellerProductInYearAsync()
    {
        var report = new PerformanceReport
        {
            Content = "Best seller product this year",
            ConstraintTimes = new List<long>(),
            NonConstraintTimes = new List<long>()
        };
        var repeatTimes = _configuration.GetValue<int>("TestRepeatTimes");

        var thisYear = DateTime.Now.Year;
        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _reportYearlyRepo.BestSellerInYearAsync(thisYear, 10).GetElapsedTimeAsync();
            report.NonConstraintTimes.Add(elapsedTime);
        }
        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _reportYearly2Repo.BestSellerInYearAsync(thisYear, 10).GetElapsedTimeAsync();
            report.ConstraintTimes.Add(elapsedTime);
        }

        return report;
    }

    public async Task<PerformanceReport> TopCustomerInYearAsync()
    {
        var report = new PerformanceReport
        {
            Content = "Top customer this year",
            ConstraintTimes = new List<long>(),
            NonConstraintTimes = new List<long>()
        };
        var repeatTimes = _configuration.GetValue<int>("TestRepeatTimes");

        var thisYear = DateTime.Now.Year;
        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _reportYearlyRepo.TopCustomerInYearAsync(thisYear, 10).GetElapsedTimeAsync();
            report.NonConstraintTimes.Add(elapsedTime);
        }
        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _reportYearly2Repo.TopCustomerInYearAsync(thisYear, 10).GetElapsedTimeAsync();
            report.ConstraintTimes.Add(elapsedTime);
        }

        return report;
    }

    public async Task<PerformanceReport> SearchProductAsync()
    {
        var report = new PerformanceReport
        {
            Content = "Search product",
            ConstraintTimes = new List<long>(),
            NonConstraintTimes = new List<long>()
        };
        var repeatTimes = _configuration.GetValue<int>("TestRepeatTimes");

        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _productRepo.SearchAsync("Chicken", 0, 20).GetElapsedTimeAsync();
            report.NonConstraintTimes.Add(elapsedTime);
        }
        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _product2Repo.SearchAsync("Chicken", 0, 20).GetElapsedTimeAsync();
            report.ConstraintTimes.Add(elapsedTime);
        }

        return report;
    }
}