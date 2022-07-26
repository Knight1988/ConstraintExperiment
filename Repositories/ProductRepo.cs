using ConstraintExperiment.Models.NonConstraint;

namespace ConstraintExperiment.Repositories;

public class ProductRepo: BaseRepo<Product>, IProductRepo
{
    private readonly NonConstraintContext _context;

    public ProductRepo(NonConstraintContext context) : base(context, context.Products, "Products")
    {
        _context = context;
    }
}