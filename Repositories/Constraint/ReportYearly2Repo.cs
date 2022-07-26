using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories.Constraint;

public class ReportYearly2Repo : IReportYearly2Repo
{
    private readonly ConstraintContext _context;

    public ReportYearly2Repo(ConstraintContext context)
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

    public async Task<List<BestSellerProduct>> BestSellerInYearAsync(int year, int product)
    {
        var first = new DateTime(year, 1, 1);
        var last = new DateTime(year + 1, 1, 1).AddDays(-1);
        var query = from o in _context.Orders
            join d in _context.Details on o.Id equals d.OrderId
            join p in _context.Products on d.ProductId equals p.Id
            where first <= o.Date && o.Date <= last
            group new {p, d} by new { p.Id, p.Name } into g
            select new BestSellerProduct()
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                Quantity = g.Sum(p => p.d.Quantity)
            };
        query = query.OrderByDescending(p => p.Quantity).Take(product);
        return await query.ToListAsync();
    }
}