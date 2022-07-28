using System.Text;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.NonConstraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.NonConstraint;

public class CustomerRepo: BaseRepo<Customer>, ICustomerRepo
{
    private readonly NonConstraintContext _context;

    public CustomerRepo(NonConstraintContext context) : base(context, context.Customers, "Customers")
    {
        _context = context;
    }

    public override async Task InsertRangeAsync(IList<Customer> objs)
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