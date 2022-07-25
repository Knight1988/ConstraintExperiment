using ConstraintExperiment.Models.Constraint;

namespace ConstraintExperiment.Interfaces;

public interface ICustomer2Repo : IBaseRepo<Customer2>
{
}

public interface IBaseRepo<in T>
{
    Task InsertRangeAsync(IEnumerable<T> customers);
    Task TruncateAsync();
}