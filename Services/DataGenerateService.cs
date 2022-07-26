using Bogus;
using ConstraintExperiment.Commons;
using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.Constraint;

namespace ConstraintExperiment.Services;

public class DataGenerateService : IDataGenerateService
{
    private readonly IConfiguration _configuration;
    private readonly ICustomer2Repo _customer2Repo;
    private readonly ICustomerRepo _customerRepo;
    private readonly IProductRepo _productRepo;
    private readonly IProduct2Repo _product2Repo;
    private readonly IProductCategoryRepo _productCategoryRepo;
    private readonly IProductCategory2Repo _productCategory2Repo;
    private readonly IOrderRepo _orderRepo;
    private readonly IOrder2Repo _order2Repo;
    private readonly IOrderDetailRepo _orderDetailRepo;
    private readonly IOrderDetail2Repo _orderDetail2Repo;

    public DataGenerateService(IConfiguration configuration, 
        ICustomerRepo customerRepo, ICustomer2Repo customer2Repo,
        IProductRepo productRepo, IProduct2Repo product2Repo,
        IProductCategoryRepo productCategoryRepo, IProductCategory2Repo productCategory2Repo,
        IOrderRepo orderRepo, IOrder2Repo order2Repo,
        IOrderDetailRepo orderDetailRepo, IOrderDetail2Repo orderDetail2Repo
        )
    {
        _configuration = configuration;
        _customerRepo = customerRepo;
        _customer2Repo = customer2Repo;
        _productRepo = productRepo;
        _product2Repo = product2Repo;
        _productCategoryRepo = productCategoryRepo;
        _productCategory2Repo = productCategory2Repo;
        _orderRepo = orderRepo;
        _order2Repo = order2Repo;
        _orderDetailRepo = orderDetailRepo;
        _orderDetail2Repo = orderDetail2Repo;
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

    public async Task FakeProductCategoryAsync()
    {
        var productCategoryId = 1;
        var count = _configuration.GetValue<double>("Fakes:ProductCategoryCount");
        var fakeProductCategory = new Faker<ProductCategory2>()
            .RuleFor(p => p.Id, f => productCategoryId++)
            .RuleFor(p => p.Name, f => f.Commerce.Categories(1)[0]);

        var chunks = Convert.ToInt32(Math.Floor(count / Constants.BatchSize));
        var leftOver = Convert.ToInt32(count - chunks * Constants.BatchSize);

        List<ProductCategory2>? productCategories;
        for (var i = 0; i < chunks; i++)
        {
            productCategories = fakeProductCategory.Generate(Constants.BatchSize);
            await _productCategory2Repo.InsertRangeAsync(productCategories);
            await _productCategoryRepo.InsertRangeAsync(productCategories);
        }
        
        productCategories = fakeProductCategory.Generate(leftOver);
        await _productCategory2Repo.InsertRangeAsync(productCategories);
        await _productCategoryRepo.InsertRangeAsync(productCategories);
    }

    public async Task FakeProductAsync()
    {
        var productId = 1;
        var count = _configuration.GetValue<double>("Fakes:ProductCount");
        var productCategoryCount = _configuration.GetValue<int>("Fakes:ProductCategoryCount");
        var fakeProduct = new Faker<Product2>()
            .RuleFor(p => p.Id, f => productId++)
            .RuleFor(p => p.CategoryId, f => f.Random.Int(1, productCategoryCount))
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Int(1, 100))
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

    public async Task FakeOrderAsync()
    {
        var orderId = 1;
        var customerCount = _configuration.GetValue<int>("Fakes:CustomerCount");
        var count = _configuration.GetValue<double>("Fakes:ProductCount");
        var fakeOrder = new Faker<Order2>()
            .RuleFor(p => p.Id, f => orderId++)
            .RuleFor(p => p.Date, f => f.Date.Past(2))
            .RuleFor(p => p.CustomerId, f => f.Random.Int(1, customerCount));
        
        var chunks = Convert.ToInt32(Math.Floor(count / Constants.BatchSize));
        var leftOver = Convert.ToInt32(count - chunks * Constants.BatchSize);

        List<Order2>? orders;
        for (var i = 0; i < chunks; i++)
        {
            orders = fakeOrder.Generate(Constants.BatchSize);
            await _order2Repo.InsertRangeAsync(orders);
            await _orderRepo.InsertRangeAsync(orders);
        }
        
        orders = fakeOrder.Generate(leftOver);
        await _order2Repo.InsertRangeAsync(orders);
        await _orderRepo.InsertRangeAsync(orders);
    }

    public async Task TruncateDatabaseAsync()
    {
        await _customerRepo.TruncateAsync();
        await _customer2Repo.TruncateAsync();
        await _productRepo.TruncateAsync();
        await _product2Repo.TruncateAsync();
        await _productCategoryRepo.TruncateAsync();
        await _productCategory2Repo.TruncateAsync();
        await _orderRepo.TruncateAsync();
        await _order2Repo.TruncateAsync();
    }
}