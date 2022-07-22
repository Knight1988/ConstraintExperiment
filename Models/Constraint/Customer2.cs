using ConstraintExperiment.Models.NonConstraint;

namespace ConstraintExperiment.Models.Constraint;

public class Customer2 : Customer
{
    public IList<Order2>? Orders { get; set; }
}