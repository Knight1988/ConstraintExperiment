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

    public PerformanceReportService(IConfiguration configuration, 
        IReportMonthRepo reportMonthRepo, IReportMonth2Repo reportMonth2Repo,
        IReportYearlyRepo reportYearlyRepo, IReportYearly2Repo reportYearly2Repo)
    {
        _configuration = configuration;
        _reportMonthRepo = reportMonthRepo;
        _reportMonth2Repo = reportMonth2Repo;
        _reportYearlyRepo = reportYearlyRepo;
        _reportYearly2Repo = reportYearly2Repo;
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
}