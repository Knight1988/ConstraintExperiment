namespace ConstraintExperiment.Interfaces;

public interface IBaseRepo<T>
{
    Task InsertRangeAsync(IList<T> customers);
    Task TruncateAsync();
}