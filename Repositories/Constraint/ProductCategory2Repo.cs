using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models.Constraint;

namespace ConstraintExperiment.Repositories.Constraint;

public class ProductCategory2Repo: BaseRepo<ProductCategory2>, IProductCategory2Repo
{
    private readonly ConstraintContext _context;

    public ProductCategory2Repo(ConstraintContext context) : base(context, context.ProductCategories, "ProductCategories")
    {
        _context = context;
    }
}