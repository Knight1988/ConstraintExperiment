using Bogus;
using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Models.Constraint;

namespace ConstraintExperiment.Services;

public class FakerService : IFakerService
{
    private readonly Faker<Customer2> _fakeCustomer;

    public FakerService()
    {
        _fakeCustomer = new Faker<Customer2>()
            .RuleFor(p => p.Id, f => f.IndexFaker)
            .RuleFor(p => p.Name, f => f.Name.FullName());
    }

    public List<Customer2> GenerateCustomer(int count)
    {
        return _fakeCustomer.Generate(count);
    }
}