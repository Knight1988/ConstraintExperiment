using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.NonConstraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.NonConstraint;

public class ProductRepo: BaseRepo<Product>, IProductRepo
{
    private readonly NonConstraintContext _context;

    public ProductRepo(NonConstraintContext context) : base(context, context.Products, "Products")
    {
        _context = context;
    }

    public Task<List<Product>> SearchAsync(string name, int skip, int take)
    {
        return _context.Products.Where(p => p.Name.Contains(name)).Skip(skip).Take(take).ToListAsync();
    }
}