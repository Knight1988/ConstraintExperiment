using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models;
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

    public async Task<List<TopCustomer>> TopCustomerInYearAsync(int year, int customer)
    {
        var first = new DateTime(year, 1, 1);
        var last = new DateTime(year + 1, 1, 1).AddDays(-1);
        var query = from o in _context.Orders
            join d in _context.Details on o.Id equals d.OrderId
            join p in _context.Products on d.ProductId equals p.Id
            join c in _context.Customers on o.CustomerId equals c.Id
            where first <= o.Date && o.Date <= last
            group new {p, d} by new { c.Id, c.Name } into g
            select new TopCustomer
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                Spent = g.Sum(p => p.d.Quantity * p.p.Price)
            };
        query = query.OrderByDescending(p => p.Spent).Take(customer);
        return await query.ToListAsync();
    }
}