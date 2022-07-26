using ConstraintExperiment.Interfaces.NonConstraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.NonConstraint;

public class ReportYearlyRepo : IReportYearlyRepo
{
    private readonly ConstraintContext _context;

    public ReportYearlyRepo(ConstraintContext context)
    {
        _context = context;
    }
    
    public async Task<int> RevenueInYearAsync(int year)
    {
        var first = new DateTime(year, 1, 1);
        var last = new DateTime(year + 1, 1, 1).AddDays(-1);
        var query = from o in _context.Orders
            join d in _context.Details on o.Id equals d.OrderId
            join p in _context.Products on d.ProductId equals p.Id
            where first <= o.Date && o.Date <= last
            select p.Price * d.Quantity;
        return await query.SumAsync(p => p);
    }
}