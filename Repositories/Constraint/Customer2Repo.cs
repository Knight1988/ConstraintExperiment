using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models.Constraint;

namespace ConstraintExperiment.Repositories.Constraint;

public class Customer2Repo: BaseRepo<Customer2>, ICustomer2Repo
{
    private readonly ConstraintContext _context;

    public Customer2Repo(ConstraintContext context) : base(context, context.Customers, "Customers")
    {
        _context = context;
    }
}