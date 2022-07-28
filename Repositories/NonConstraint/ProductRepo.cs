using System.Text;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.NonConstraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.NonConstraint;

public class ProductRepo : BaseRepo<Product>, IProductRepo
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

    public override async Task InsertRangeAsync(IList<Product> objs)
    {
        if (objs.Count == 0) return;
        if (!BaseContext.IsMySql)
        {
            await base.InsertRangeAsync(objs);
            return;
        }

        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append("INSERT INTO Products(Id, CategoryId, Name, Description, Price) VALUES ");
        foreach (var product in objs)
        {
            sqlBuilder.Append(
                $"({product.Id}, {product.CategoryId}, '{product.Name.Replace("'", "\\'")}', '{product.Description.Replace("'", "\\'")}', {product.Price}),");
        }

        var sql = sqlBuilder.ToString().TrimEnd(',');
        await _context.Database.ExecuteSqlRawAsync(sql);
    }
}