using Bogus;
using ConstraintExperiment.Commons;
using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Models.Constraint;
using ConstraintExperiment.Repositories;

namespace ConstraintExperiment.Services;

public class DataGenerateService : IDataGenerateService
{
    private readonly IConfiguration _configuration;
    private readonly ICustomer2Repo _customer2Repo;

    public DataGenerateService(IConfiguration configuration, ICustomer2Repo customer2Repo)
    {
        _configuration = configuration;
        _customer2Repo = customer2Repo;
    }
    
    public async Task FakeCustomerAsync()
    {
        var customerId = 1;
        var count = _configuration.GetValue<double>("Fakes:CustomerCount");
        var fakeCustomer = new Faker<Customer2>()
            .RuleFor(p => p.Id, f => customerId++)
            .RuleFor(p => p.Name, f => f.Name.FullName());

        var chunks = Convert.ToInt32(Math.Floor(count / Constants.BatchSize));
        var leftOver = count - chunks * Constants.BatchSize;

        for (int i = 0; i < chunks; i++)
        {
            var customers = fakeCustomer.Generate(Constants.BatchSize);
            await _customer2Repo.InsertRangeAsync(customers);
        }
    }

    public async Task TruncateDatabaseAsync()
    {
        await _customer2Repo.TruncateAsync();
    }
}