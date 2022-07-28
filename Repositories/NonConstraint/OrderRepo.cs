using System.Text;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.NonConstraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.NonConstraint;

public class OrderRepo: BaseRepo<Order>, IOrderRepo
{
    private readonly NonConstraintContext _context;

    public OrderRepo(NonConstraintContext context) : base(context, context.Orders, "Orders")
    {
        _context = context;
    }
    
    public override async Task InsertRangeAsync(IList<Order> objs)
    {
        if (objs.Count == 0) return;
        if (!BaseContext.IsMySql)
        {
            await base.InsertRangeAsync(objs);
            return;
        }
        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append("INSERT INTO Orders(Id, CustomerId, Date) VALUES ");
        foreach (var order in objs)
        {
            sqlBuilder.Append($"({order.Id}, {order.CustomerId}, '{order.Date.ToString("yyyy-MM-dd")}'),");
        }

        var sql = sqlBuilder.ToString().TrimEnd(',');
        await _context.Database.ExecuteSqlRawAsync(sql);
    }
}