using ConstraintExperiment.Models.NonConstraint;

namespace ConstraintExperiment.Models.Constraint;

public class Product2 : Product
{
    public ProductCategory2? Category { get; set; }
    public IList<OrderDetail2> OrderDetails { get; set; }
}