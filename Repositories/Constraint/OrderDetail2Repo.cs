using System.Text;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models.Constraint;
using ConstraintExperiment.Models.NonConstraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.Constraint;

public class OrderDetail2Repo: BaseRepo<OrderDetail2>, IOrderDetail2Repo
{
    private readonly ConstraintContext _context;

    public OrderDetail2Repo(ConstraintContext context) : base(context, context.Details, "Details")
    {
        _context = context;
    }

    public override async Task InsertRangeAsync(IList<OrderDetail2> objs)
    {
        if (objs.Count == 0) return;
        if (!BaseContext.IsMySql)
        {
            await base.InsertRangeAsync(objs);
            return;
        }
        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append("INSERT INTO Details(Id, OrderId, ProductId, Quantity) VALUES ");
        foreach (var detail in objs)
        {
            sqlBuilder.Append($"({detail.Id}, {detail.OrderId}, {detail.ProductId}, {detail.Quantity}),");
        }

        var sql = sqlBuilder.ToString().TrimEnd(',');
        await _context.Database.ExecuteSqlRawAsync(sql);
    }
}