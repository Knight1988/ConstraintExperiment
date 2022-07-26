using ConstraintExperiment.Models.Constraint;

namespace ConstraintExperiment.Interfaces.Constraint;

public interface IProduct2Repo : IBaseRepo<Product2>
{
    Task<List<Product2>> SearchAsync(string name, int skip, int take);
}