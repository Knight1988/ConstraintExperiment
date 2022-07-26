﻿using ConstraintExperiment.Models.NonConstraint;
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
        // connect to sql server with connection string from app settings
        options.UseSqlServer(_configuration.GetConnectionString("NonConstraintDb"));
    }
}