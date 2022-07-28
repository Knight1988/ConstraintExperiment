using System.Text;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models.Constraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.Constraint;

public class Customer2Repo: BaseRepo<Customer2>, ICustomer2Repo
{
    private readonly ConstraintContext _context;

    public Customer2Repo(ConstraintContext context) : base(context, context.Customers, "Customers")
    {
        _context = context;
    }
    
    public override async Task InsertRangeAsync(IList<Customer2> objs)
    {
        if (objs.Count == 0) return;
        if (!BaseContext.IsMySql)
        {
            await base.InsertRangeAsync(objs);
            return;
        }
        var sqlBuilder = new StringBuilder();
        sqlBuilder.Append("INSERT INTO Customers(Id, Name) VALUES ");
        foreach (var customer in objs)
        {
            sqlBuilder.Append($"({customer.Id}, '{customer.Name.Replace("'", "\\'")}'),");
        }

        var sql = sqlBuilder.ToString().TrimEnd(',');
        await _context.Database.ExecuteSqlRawAsync(sql);
    }
}