using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.NonConstraint;

namespace ConstraintExperiment.Repositories.NonConstraint;

public class OrderDetailRepo: BaseRepo<OrderDetail>, IOrderDetailRepo
{
    private readonly NonConstraintContext _context;

    public OrderDetailRepo(NonConstraintContext context) : base(context, context.Details, "Details")
    {
        _context = context;
    }
}