using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models.Constraint;

namespace ConstraintExperiment.Repositories.Constraint;

public class Order2Repo: BaseRepo<Order2>, IOrder2Repo
{
    private readonly ConstraintContext _context;

    public Order2Repo(ConstraintContext context) : base(context, context.Orders, "Orders")
    {
        _context = context;
    }
}