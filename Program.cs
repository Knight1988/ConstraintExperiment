using ConstraintExperiment;
using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Repositories;
using ConstraintExperiment.Services;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        
        services.AddDbContext<NonConstraintContext>();
        services.AddDbContext<ConstraintContext>();

        services.AddScoped<IDataGenerateService, DataGenerateService>();
        
        services.AddScoped<ICustomerRepo, CustomerRepo>();
        services.AddScoped<ICustomer2Repo, Customer2Repo>();
        services.AddScoped<IProductRepo, ProductRepo>();
        services.AddScoped<IProduct2Repo, Product2Repo>();
    })
    .Build();

await host.RunAsync();