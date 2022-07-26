using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models.Constraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.Constraint;

public class Product2Repo: BaseRepo<Product2>, IProduct2Repo
{
    private readonly ConstraintContext _context;

    public Product2Repo(ConstraintContext context) : base(context, context.Products, "Products")
    {
        _context = context;
    }

    public Task<List<Product2>> SearchAsync(string name, int skip, int take)
    {
        return _context.Products.Where(p => p.Name.Contains(name)).Skip(skip).Take(take).ToListAsync();
    }
}