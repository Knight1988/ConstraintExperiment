using ConstraintExperiment;
using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Repositories;
using ConstraintExperiment.Repositories.Constraint;
using ConstraintExperiment.Repositories.NonConstraint;
using ConstraintExperiment.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        
        services.AddDbContext<NonConstraintContext>();
        services.AddDbContext<ConstraintContext>();

        services.AddScoped<IDataGenerateService, DataGenerateService>();
        services.AddScoped<IPerformanceReportService, PerformanceReportService>();
        
        services.AddScoped<ICustomerRepo, CustomerRepo>();
        services.AddScoped<ICustomer2Repo, Customer2Repo>();
        services.AddScoped<IProductRepo, ProductRepo>();
        services.AddScoped<IProduct2Repo, Product2Repo>();
        services.AddScoped<IProductCategoryRepo, ProductCategoryRepo>();
        services.AddScoped<IProductCategory2Repo, ProductCategory2Repo>();
        services.AddScoped<IOrderRepo, OrderRepo>();
        services.AddScoped<IOrder2Repo, Order2Repo>();
        services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();
        services.AddScoped<IOrderDetail2Repo, OrderDetail2Repo>();
        services.AddScoped<IReportMonthRepo, ReportMonthRepo>();
        services.AddScoped<IReportMonth2Repo, ReportMonth2Repo>();
        services.AddScoped<IReportYearlyRepo, ReportYearlyRepo>();
        services.AddScoped<IReportYearly2Repo, ReportYearly2Repo>();
    })
    .Build();

await host.RunAsync();