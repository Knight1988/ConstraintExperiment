using System.Text;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.NonConstraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.NonConstraint;

public class ProductCategoryRepo: BaseRepo<ProductCategory>, IProductCategoryRepo
{
    private readonly NonConstraintContext _context;

    public ProductCategoryRepo(NonConstraintContext context) : base(context, context.ProductCategories, "ProductCategories")
    {
        _context = context;
    }

    public override async Task InsertRangeAsync(IList<ProductCategory> objs)
    {
        if (objs.Count == 0) return;
        if (!BaseContext.IsMySql)
        {
            await base.InsertRangeAsync(objs);
            return;
        }

        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append("INSERT INTO ProductCategories(Id, Name) VALUES ");
        foreach (var product in objs)
        {
            sqlBuilder.Append(
                $"({product.Id}, '{product.Name.Replace("'", "\\'")}'),");
        }

        var sql = sqlBuilder.ToString().TrimEnd(',');
        await _context.Database.ExecuteSqlRawAsync(sql);
    }
}