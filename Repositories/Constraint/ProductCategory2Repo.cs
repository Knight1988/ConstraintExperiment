using System.Text;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models.Constraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.Constraint;

public class ProductCategory2Repo: BaseRepo<ProductCategory2>, IProductCategory2Repo
{
    private readonly ConstraintContext _context;

    public ProductCategory2Repo(ConstraintContext context) : base(context, context.ProductCategories, "ProductCategories")
    {
        _context = context;
    }

    public override async Task InsertRangeAsync(IList<ProductCategory2> objs)
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