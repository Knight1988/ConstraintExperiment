using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Models.Constraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories;

public class Customer2Repo: ICustomer2Repo
{
    private readonly ConstraintContext _context;

    public Customer2Repo(ConstraintContext context)
    {
        _context = context;
    }
    
    public async Task InsertRangeAsync(IEnumerable<Customer2> customers)
    {
        await _context.Customers.AddRangeAsync(customers);
        await _context.SaveChangesAsync();
    }

    public async Task TruncateAsync()
    {
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM Customers");
    }
}