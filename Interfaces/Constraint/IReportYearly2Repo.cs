namespace ConstraintExperiment.Interfaces.Constraint;

public interface IReportYearly2Repo
{
    Task<int> RevenueInYearAsync(int year);
}