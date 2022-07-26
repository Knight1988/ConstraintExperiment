using ConstraintExperiment.Models.NonConstraint;

namespace ConstraintExperiment.Interfaces.NonConstraint;

public interface IProductRepo : IBaseRepo<Product>
{
    Task<List<Product>> SearchAsync(string name, int skip, int take);
}