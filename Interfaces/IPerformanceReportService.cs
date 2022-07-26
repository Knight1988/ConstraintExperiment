using ConstraintExperiment.Models;

namespace ConstraintExperiment.Interfaces;

public interface IPerformanceReportService
{
    Task<string> WriteToFileAsync(IEnumerable<PerformanceReport> reports);
    Task<PerformanceReport> RevenueLastMonthAsync();
    Task<PerformanceReport> RevenueInYearAsync();
    Task<PerformanceReport> BestSellerProductInYearAsync();
    Task<PerformanceReport> TopCustomerInYearAsync();
    Task<PerformanceReport> SearchProductAsync();
}