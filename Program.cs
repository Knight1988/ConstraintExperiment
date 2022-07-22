using ConstraintExperiment;
using ConstraintExperiment.Repositories;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddDbContext<NonConstraintContext>();
        services.AddDbContext<ConstraintContext>();
    })
    .Build();

await host.RunAsync();