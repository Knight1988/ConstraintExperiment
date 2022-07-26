namespace ConstraintExperiment.Interfaces.Constraint;

public interface IReportMonth2Repo
{
    Task<int> RevenueMonthlyAsync(int lastMonth);
}