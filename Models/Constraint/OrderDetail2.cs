using ConstraintExperiment.Models.NonConstraint;

namespace ConstraintExperiment.Models.Constraint;

public class OrderDetail2 : OrderDetail
{
    public Order2? Order { get; set; }
    public Product2? Product { get; set; }
}