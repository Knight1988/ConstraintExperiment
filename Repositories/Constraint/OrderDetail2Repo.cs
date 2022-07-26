using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models.Constraint;

namespace ConstraintExperiment.Repositories.Constraint;

public class OrderDetail2Repo: BaseRepo<OrderDetail2>, IOrderDetail2Repo
{
    private readonly ConstraintContext _context;

    public OrderDetail2Repo(ConstraintContext context) : base(context, context.Details, "Details")
    {
        _context = context;
    }
}