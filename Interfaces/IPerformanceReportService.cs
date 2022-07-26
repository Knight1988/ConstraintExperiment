using ConstraintExperiment.Models;

namespace ConstraintExperiment.Interfaces;

public interface IPerformanceReportService
{
    Task<PerformanceReport> RevenueLastMonthAsync();
}