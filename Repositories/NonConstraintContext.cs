using ConstraintExperiment.Models.NonConstraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories;

public class NonConstraintContext : DbContext
{
    private readonly IConfiguration _configuration;

    public NonConstraintContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> Details { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var contextTimeout = _configuration.GetValue<int>("ContextTimeout");
        var provider = _configuration.GetValue<string>("Provider");
        switch (provider)
        {
            case "MSSQL":
            {
                options.UseSqlServer(_configuration.GetConnectionString("NonConstraintMssql"), builder => 
                    builder.CommandTimeout(contextTimeout));
                break;
            }
            case "Postgres":
            {
                options.UseNpgsql(_configuration.GetConnectionString("NonConstraintPostgres"), builder =>
                    builder.CommandTimeout(contextTimeout));
                // fix error: Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone'
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                break;
            }
            default:
                throw new Exception($"Unsupported provider: {provider}");
        }
    }
}