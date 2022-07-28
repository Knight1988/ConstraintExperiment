using System.Text;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models.Constraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.Constraint;

public class Product2Repo : BaseRepo<Product2>, IProduct2Repo
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

    public override async Task InsertRangeAsync(IList<Product2> objs)
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