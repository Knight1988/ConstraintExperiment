using System.Text;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.NonConstraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.NonConstraint;

public class OrderDetailRepo: BaseRepo<OrderDetail>, IOrderDetailRepo
{
    private readonly NonConstraintContext _context;

    public OrderDetailRepo(NonConstraintContext context) : base(context, context.Details, "Details")
    {
        _context = context;
    }

    public override async Task InsertRangeAsync(IList<OrderDetail> objs)
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