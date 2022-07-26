using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.NonConstraint;

namespace ConstraintExperiment.Repositories.NonConstraint;

public class ProductCategoryRepo: BaseRepo<ProductCategory>, IProductCategoryRepo
{
    private readonly NonConstraintContext _context;

    public ProductCategoryRepo(NonConstraintContext context) : base(context, context.ProductCategories, "ProductCategories")
    {
        _context = context;
    }
}