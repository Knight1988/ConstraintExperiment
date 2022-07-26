using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models.Constraint;

namespace ConstraintExperiment.Repositories.Constraint;

public class Product2Repo: BaseRepo<Product2>, IProduct2Repo
{
    private readonly ConstraintContext _context;

    public Product2Repo(ConstraintContext context) : base(context, context.Products, "Products")
    {
        _context = context;
    }
}