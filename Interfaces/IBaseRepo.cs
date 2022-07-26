namespace ConstraintExperiment.Interfaces;

public interface IBaseRepo<in T>
{
    Task InsertRangeAsync(IEnumerable<T> customers);
    Task TruncateAsync();
}