using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.NonConstraint;

namespace ConstraintExperiment.Repositories.NonConstraint;

public class OrderRepo: BaseRepo<Order>, IOrderRepo
{
    private readonly NonConstraintContext _context;

    public OrderRepo(NonConstraintContext context) : base(context, context.Orders, "Orders")
    {
        _context = context;
    }
}