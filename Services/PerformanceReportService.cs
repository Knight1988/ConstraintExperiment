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

    public PerformanceReportService(IConfiguration configuration, 
        IReportMonthRepo reportMonthRepo, IReportMonth2Repo reportMonth2Repo)
    {
        _configuration = configuration;
        _reportMonthRepo = reportMonthRepo;
        _reportMonth2Repo = reportMonth2Repo;
    }
    
    public async Task<PerformanceReport> RevenueLastMonthAsync()
    {
        var report = new PerformanceReport
        {
            Content = "Get revenue last month",
            ConstaintTimes = new List<long>(),
            NonConstaintTimes = new List<long>()
        };
        var repeatTimes = _configuration.GetValue<int>("TestRepeatTimes");

        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _reportMonthRepo.RevenueMonthlyAsync(3).GetElapsedTimeAsync();
            report.NonConstaintTimes.Add(elapsedTime);
        }
        for (var i = 0; i < repeatTimes; i++)
        {
            var elapsedTime = await _reportMonth2Repo.RevenueMonthlyAsync(3).GetElapsedTimeAsync();
            report.ConstaintTimes.Add(elapsedTime);
        }

        return report;
    }
}