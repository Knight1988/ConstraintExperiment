using System.Text;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models.Constraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.Constraint;

public class Order2Repo: BaseRepo<Order2>, IOrder2Repo
{
    private readonly ConstraintContext _context;

    public Order2Repo(ConstraintContext context) : base(context, context.Orders, "Orders")
    {
        _context = context;
    }
    
    public override async Task InsertRangeAsync(IList<Order2> objs)
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