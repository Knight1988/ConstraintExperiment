using ConstraintExperiment.Models;

namespace ConstraintExperiment.Interfaces.NonConstraint;

public interface IReportYearlyRepo
{
    Task<int> RevenueInYearAsync(int year);
    Task<List<BestSellerProduct>> BestSellerInYearAsync(int year, int product);
}