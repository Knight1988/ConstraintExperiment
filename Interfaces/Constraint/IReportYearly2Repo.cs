using ConstraintExperiment.Models;

namespace ConstraintExperiment.Interfaces.Constraint;

public interface IReportYearly2Repo
{
    Task<int> RevenueInYearAsync(int year);
    Task<List<BestSellerProduct>> BestSellerInYearAsync(int year, int product);
    Task<List<TopCustomer>> TopCustomerInYearAsync(int year, int customer);
}