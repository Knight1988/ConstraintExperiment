namespace ConstraintExperiment.Interfaces.NonConstraint;

public interface IReportYearlyRepo
{
    Task<int> RevenueInYearAsync(int year);
}