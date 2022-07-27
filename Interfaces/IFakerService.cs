using ConstraintExperiment.Models.Constraint;

namespace ConstraintExperiment.Interfaces;

public interface IFakerService
{
    List<Customer2> GenerateCustomer(int count);
}