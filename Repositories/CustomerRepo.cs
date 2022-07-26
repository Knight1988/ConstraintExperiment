using ConstraintExperiment.Models.NonConstraint;

namespace ConstraintExperiment.Repositories;

public class CustomerRepo: BaseRepo<Customer>, ICustomerRepo
{
    private readonly NonConstraintContext _context;

    public CustomerRepo(NonConstraintContext context) : base(context, context.Customers, "Customers")
    {
        _context = context;
    }
}