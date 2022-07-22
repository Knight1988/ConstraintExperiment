using ConstraintExperiment.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories;

public class ConstraintContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ConstraintContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<Product2> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sql server with connection string from app settings
        options.UseSqlServer(_configuration.GetConnectionString("ConstraintDb"));
    }
}