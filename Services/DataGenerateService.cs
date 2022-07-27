using Bogus;
using ConstraintExperiment.Commons;
using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.Constraint;
using ConstraintExperiment.Models.NonConstraint;
using ConstraintExperiment.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

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
    private readonly ILogger<DataGenerateService> _logger;
    private readonly ConstraintContext _constraintContext;
    private readonly NonConstraintContext _nonConstraintContext;

    public DataGenerateService(IConfiguration configuration, ILogger<DataGenerateService> logger,
        ConstraintContext constraintContext, NonConstraintContext nonConstraintContext,
        ICustomerRepo customerRepo, ICustomer2Repo customer2Repo,
        IProductRepo productRepo, IProduct2Repo product2Repo,
        IProductCategoryRepo productCategoryRepo, IProductCategory2Repo productCategory2Repo,
        IOrderRepo orderRepo, IOrder2Repo order2Repo,
        IOrderDetailRepo orderDetailRepo, IOrderDetail2Repo orderDetail2Repo
        )
    {
        _logger = logger;
        _configuration = configuration;
        _constraintContext = constraintContext;
        _nonConstraintContext = nonConstraintContext;
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

    public async Task MigrateDatabaseAsync()
    {
        await _constraintContext.Database.MigrateAsync();
        await _nonConstraintContext.Database.MigrateAsync();
    }

    public async Task FakeCustomerAsync()
    {
        var customerId = 1;
        var count = _configuration.GetValue<double>("Fakes:CustomerCount");
        var fakeCustomer = new Faker<Customer2>()
            .RuleFor(p => p.Id, _ => customerId++)
            .RuleFor(p => p.Name, f => f.Name.FullName());

        var chunks = Convert.ToInt32(Math.Floor(count / Constants.BatchSize));
        var leftOver = Convert.ToInt32(count - chunks * Constants.BatchSize);

        List<Customer2>? customers2;
        List<Customer>? customers;
        _logger.LogInformation("Insert customer: {Percent:P}", 0);
        for (var i = 0; i < chunks; i++)
        {
            customers2 = fakeCustomer.Generate(Constants.BatchSize);
            customers = customers2.ConvertAll(p => (Customer)p);
            await _customer2Repo.InsertRangeAsync(customers2);
            await _customerRepo.InsertRangeAsync(customers);
            _logger.LogInformation("Insert customer: {Percent:P}", (double) i / chunks);
        }
        
        customers2 = fakeCustomer.Generate(leftOver);
        customers = customers2.ConvertAll(p => (Customer)p);
        await _customer2Repo.InsertRangeAsync(customers2);
        await _customerRepo.InsertRangeAsync(customers);
        _logger.LogInformation("Insert customer: {Percent:P}", 1);
    }

    public async Task FakeProductCategoryAsync()
    {
        var productCategoryId = 1;
        var count = _configuration.GetValue<double>("Fakes:ProductCategoryCount");
        var fakeProductCategory = new Faker<ProductCategory2>()
            .RuleFor(p => p.Id, _ => productCategoryId++)
            .RuleFor(p => p.Name, f => f.Commerce.Categories(1)[0]);

        var chunks = Convert.ToInt32(Math.Floor(count / Constants.BatchSize));
        var leftOver = Convert.ToInt32(count - chunks * Constants.BatchSize);

        List<ProductCategory2>? productCategories2;
        List<ProductCategory>? productCategories;
        _logger.LogInformation("Insert product categories: {Percent:P}", 0);
        for (var i = 0; i < chunks; i++)
        {
            productCategories2 = fakeProductCategory.Generate(Constants.BatchSize);
            productCategories = productCategories2.ConvertAll(p => (ProductCategory) p);
            await _productCategory2Repo.InsertRangeAsync(productCategories2);
            await _productCategoryRepo.InsertRangeAsync(productCategories);
            _logger.LogInformation("Insert product categories: {Percent:P}", (double) i / chunks);
        }
        
        productCategories2 = fakeProductCategory.Generate(leftOver);
        productCategories = productCategories2.ConvertAll(p => (ProductCategory) p);
        await _productCategory2Repo.InsertRangeAsync(productCategories2);
        await _productCategoryRepo.InsertRangeAsync(productCategories);
        _logger.LogInformation("Insert product categories: {Percent:P}", 1);
    }

    public async Task FakeProductAsync()
    {
        var productId = 1;
        var count = _configuration.GetValue<double>("Fakes:ProductCount");
        var productCategoryCount = _configuration.GetValue<int>("Fakes:ProductCategoryCount");
        var fakeProduct = new Faker<Product2>()
            .RuleFor(p => p.Id, _ => productId++)
            .RuleFor(p => p.CategoryId, f => f.Random.Int(1, productCategoryCount))
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Int(1, 100))
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription());

        var chunks = Convert.ToInt32(Math.Floor(count / Constants.BatchSize));
        var leftOver = Convert.ToInt32(count - chunks * Constants.BatchSize);

        List<Product2>? products2;
        List<Product>? products;
        _logger.LogInformation("Insert products: {Percent:P}", 0);
        for (var i = 0; i < chunks; i++)
        {
            products2 = fakeProduct.Generate(Constants.BatchSize);
            products = products2.ConvertAll(p => (Product)p);
            await _product2Repo.InsertRangeAsync(products2);
            await _productRepo.InsertRangeAsync(products);
            _logger.LogInformation("Insert products: {Percent:P}", (double) i / chunks);
        }
        
        products2 = fakeProduct.Generate(leftOver);
        products = products2.ConvertAll(p => (Product)p);
        await _product2Repo.InsertRangeAsync(products2);
        await _productRepo.InsertRangeAsync(products);
        _logger.LogInformation("Insert products: {Percent:P}", 1);
    }

    public async Task FakeOrderAsync()
    {
        var orderId = 1;
        var customerCount = _configuration.GetValue<int>("Fakes:CustomerCount");
        var count = _configuration.GetValue<double>("Fakes:OrderCount");
        var fakeOrder = new Faker<Order2>()
            .RuleFor(p => p.Id, _ => orderId++)
            .RuleFor(p => p.Date, f => f.Date.Past(2))
            .RuleFor(p => p.CustomerId, f => f.Random.Int(1, customerCount));
        
        var chunks = Convert.ToInt32(Math.Floor(count / Constants.BatchSize));
        var leftOver = Convert.ToInt32(count - chunks * Constants.BatchSize);

        List<Order2>? orders2;
        List<Order>? orders;
        _logger.LogInformation("Insert orders: {Percent:P}", 0);
        for (var i = 0; i < chunks; i++)
        {
            orders2 = fakeOrder.Generate(Constants.BatchSize);
            orders = orders2.ConvertAll(p => (Order)p);
            await _order2Repo.InsertRangeAsync(orders2);
            await _orderRepo.InsertRangeAsync(orders);
            _logger.LogInformation("Insert orders: {Percent:P}", (double) i / chunks);
        }
        
        orders2 = fakeOrder.Generate(leftOver);
        orders = orders2.ConvertAll(p => (Order)p);
        await _order2Repo.InsertRangeAsync(orders2);
        await _orderRepo.InsertRangeAsync(orders);
        _logger.LogInformation("Insert orders: {Percent:P}", 1);
    }

    public async Task FakeOrderDetailAsync()
    {
        var orderId = 1;
        var orderDetailId = 1;
        var productCount = _configuration.GetValue<int>("Fakes:ProductCount");
        var count = _configuration.GetValue<double>("Fakes:OrderCount");
        var fakeOrderDetail = new Faker<OrderDetail2>()
            .RuleFor(p => p.Id, _ => orderDetailId++)
            .RuleFor(p => p.OrderId, _ => orderId)
            .RuleFor(p => p.ProductId, f => f.Random.Int(1, productCount))
            .RuleFor(p => p.Quantity, f => f.Random.Int(1, 10));

        _logger.LogInformation("Insert order details: {Percent:P}", 0);
        var orderDetails2 = new List<OrderDetail2>();
        List<OrderDetail> orderDetails;
        var faker = new Faker();
        for (orderId = 1; orderId <= count; orderId++)
        {
            orderDetails2.AddRange(fakeOrderDetail.Generate(faker.Random.Int(1, 10)));

            if (orderId % Constants.BatchSize != 0) continue;

            orderDetails = orderDetails2.ConvertAll(p => (OrderDetail) p);
            await _orderDetail2Repo.InsertRangeAsync(orderDetails2);
            await _orderDetailRepo.InsertRangeAsync(orderDetails);
            orderDetails2 = new List<OrderDetail2>();
            _logger.LogInformation("Insert order details: {Percent:P}", orderId / count);
        }
        
        orderDetails = orderDetails2.ConvertAll(p => (OrderDetail) p);
        await _orderDetail2Repo.InsertRangeAsync(orderDetails2);
        await _orderDetailRepo.InsertRangeAsync(orderDetails);
        _logger.LogInformation("Insert order details: {Percent:P}", 1);
    }

    public async Task TearDownDatabaseAsync()
    {
        await _constraintContext.GetInfrastructure().GetService<IMigrator>().MigrateAsync("0");
        await _nonConstraintContext.GetInfrastructure().GetService<IMigrator>().MigrateAsync("0");
    }
}