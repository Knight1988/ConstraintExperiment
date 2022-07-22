using ConstraintExperiment.Models.NonConstraint;

namespace ConstraintExperiment.Models.Constraint;

public class Order2 : Order
{
    public Customer2? Customer { get; set; }
    public IList<OrderDetail2>? OrderDetails { get; set; }
}