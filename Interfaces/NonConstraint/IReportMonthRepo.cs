namespace ConstraintExperiment.Interfaces.NonConstraint;

public interface IReportMonthRepo
{
    Task<int> RevenueMonthlyAsync(int lastMonth);
}