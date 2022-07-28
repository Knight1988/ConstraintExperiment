using ConstraintExperiment.Models.Constraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories;

public class ConstraintContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ConstraintContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<Customer2> Customers { get; set; }
    public DbSet<Order2> Orders { get; set; }
    public DbSet<OrderDetail2> Details { get; set; }
    public DbSet<Product2> Products { get; set; }
    public DbSet<ProductCategory2> ProductCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var contextTimeout = _configuration.GetValue<int>("ContextTimeout");
        var provider = _configuration.GetValue<string>("Provider").ToLowerInvariant();
        switch (provider)
        {
            case "mssql":
            {
                options.UseSqlServer(_configuration.GetConnectionString("ConstraintMssql"), builder => 
                    builder.CommandTimeout(contextTimeout));
                break;
            }
            case "postgres":
            {
                options.UseNpgsql(_configuration.GetConnectionString("ConstraintPostgres"), builder =>
                    builder.CommandTimeout(contextTimeout));
                // fix error: Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone'
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                break;
            }
            case "mysql":
            {
                options.UseMySQL(_configuration.GetConnectionString("ConstraintMysql"), builder =>
                    builder.CommandTimeout(contextTimeout));
                break;
            }
            default:
                throw new Exception($"Unsupported provider: {provider}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory2>()
            .HasMany<Product2>(category2 => category2.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Customer2>()
            .HasMany<Order2>(category2 => category2.Orders)
            .WithOne(p => p.Customer)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Order2>()
            .HasMany<OrderDetail2>(category2 => category2.OrderDetails)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Product2>()
            .HasMany<OrderDetail2>(category2 => category2.OrderDetails)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}