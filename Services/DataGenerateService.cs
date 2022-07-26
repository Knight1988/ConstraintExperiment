using Bogus;
using ConstraintExperiment.Commons;
using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.Constraint;
using ConstraintExperiment.Repositories;

namespace ConstraintExperiment.Services;

public class DataGenerateService : IDataGenerateService
{
    private readonly IConfiguration _configuration;
    private readonly ICustomer2Repo _customer2Repo;
    private readonly ICustomerRepo _customerRepo;
    private readonly IProductRepo _productRepo;
    private readonly IProduct2Repo _product2Repo;

    public DataGenerateService(IConfiguration configuration, 
        ICustomerRepo customerRepo, ICustomer2Repo customer2Repo,
        IProductRepo productRepo, IProduct2Repo product2Repo
        )
    {
        _configuration = configuration;
        _customerRepo = customerRepo;
        _customer2Repo = customer2Repo;
        _productRepo = productRepo;
        _product2Repo = product2Repo;
    }
    
    public async Task FakeCustomerAsync()
    {
        var customerId = 1;
        var count = _configuration.GetValue<double>("Fakes:CustomerCount");
        var fakeCustomer = new Faker<Customer2>()
            .RuleFor(p => p.Id, f => customerId++)
            .RuleFor(p => p.Name, f => f.Name.FullName());

        var chunks = Convert.ToInt32(Math.Floor(count / Constants.BatchSize));
        var leftOver = Convert.ToInt32(count - chunks * Constants.BatchSize);

        List<Customer2>? customers;
        for (var i = 0; i < chunks; i++)
        {
            customers = fakeCustomer.Generate(Constants.BatchSize);
            await _customer2Repo.InsertRangeAsync(customers);
            await _customerRepo.InsertRangeAsync(customers);
        }
        
        customers = fakeCustomer.Generate(leftOver);
        await _customer2Repo.InsertRangeAsync(customers);
        await _customerRepo.InsertRangeAsync(customers);
    }

    public async Task FakeProductAsync()
    {
        var productId = 1;
        var count = _configuration.GetValue<double>("Fakes:ProductCount");
        var fakeProduct = new Faker<Product2>()
            .RuleFor(p => p.Id, f => productId++)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription());

        var chunks = Convert.ToInt32(Math.Floor(count / Constants.BatchSize));
        var leftOver = Convert.ToInt32(count - chunks * Constants.BatchSize);

        List<Product2>? products;
        for (var i = 0; i < chunks; i++)
        {
            products = fakeProduct.Generate(Constants.BatchSize);
            await _product2Repo.InsertRangeAsync(products);
            await _productRepo.InsertRangeAsync(products);
        }
        
        products = fakeProduct.Generate(leftOver);
        await _product2Repo.InsertRangeAsync(products);
        await _productRepo.InsertRangeAsync(products);
    }

    public async Task TruncateDatabaseAsync()
    {
        await _customerRepo.TruncateAsync();
        await _customer2Repo.TruncateAsync();
    }
}