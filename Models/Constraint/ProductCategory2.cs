using ConstraintExperiment.Models.NonConstraint;

namespace ConstraintExperiment.Models.Constraint;

public class ProductCategory2 : ProductCategory
{
    public IList<Product2>? Products { get; set; }
}