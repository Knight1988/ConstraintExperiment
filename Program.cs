using ConstraintExperiment;
using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Repositories;
using ConstraintExperiment.Repositories.Constraint;
using ConstraintExperiment.Repositories.NonConstraint;
using ConstraintExperiment.Services;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        
        services.AddDbContext<NonConstraintContext>(ServiceLifetime.Transient);
        services.AddDbContext<ConstraintContext>(ServiceLifetime.Transient);

        services.AddSingleton<IDataGenerateService, DataGenerateService>();
        services.AddSingleton<IPerformanceReportService, PerformanceReportService>();
        
        services.AddSingleton<ICustomerRepo, CustomerRepo>();
        services.AddSingleton<ICustomer2Repo, Customer2Repo>();
        services.AddSingleton<IProductRepo, ProductRepo>();
        services.AddSingleton<IProduct2Repo, Product2Repo>();
        services.AddSingleton<IProductCategoryRepo, ProductCategoryRepo>();
        services.AddSingleton<IProductCategory2Repo, ProductCategory2Repo>();
        services.AddSingleton<IOrderRepo, OrderRepo>();
        services.AddSingleton<IOrder2Repo, Order2Repo>();
        services.AddSingleton<IOrderDetailRepo, OrderDetailRepo>();
        services.AddSingleton<IOrderDetail2Repo, OrderDetail2Repo>();
        services.AddSingleton<IReportMonthRepo, ReportMonthRepo>();
        services.AddSingleton<IReportMonth2Repo, ReportMonth2Repo>();
        services.AddSingleton<IReportYearlyRepo, ReportYearlyRepo>();
        services.AddSingleton<IReportYearly2Repo, ReportYearly2Repo>();
    })
    .UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
    )
    .Build();

await host.RunAsync();