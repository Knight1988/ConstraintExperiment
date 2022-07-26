using ConstraintExperiment.Interfaces.NonConstraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.NonConstraint;

public class ReportMonthRepo : IReportMonthRepo
{
    private readonly ConstraintContext _context;

    public ReportMonthRepo(ConstraintContext context)
    {
        _context = context;
    }
    
    public async Task<int> RevenueMonthlyAsync(int lastMonth)
    {
        var today = DateTime.Today;
        var month = new DateTime(today.Year, today.Month, 1);
        var first = month.AddMonths(-lastMonth);
        var last = month.AddDays(-1);
        var query = from o in _context.Orders
            join d in _context.Details on o.Id equals d.OrderId
            join p in _context.Products on d.ProductId equals p.Id
            where first <= o.Date && o.Date <= last
            select p.Price * d.Quantity;
        return await query.SumAsync(p => p);
    }
}