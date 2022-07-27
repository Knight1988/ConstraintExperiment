﻿using Bogus;
using ConstraintExperiment.Commons;
using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models.Constraint;
using ConstraintExperiment.Models.NonConstraint;

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

    public DataGenerateService(IConfiguration configuration, ILogger<DataGenerateService> logger,
        ICustomerRepo customerRepo, ICustomer2Repo customer2Repo,
        IProductRepo productRepo, IProduct2Repo product2Repo,
        IProductCategoryRepo productCategoryRepo, IProductCategory2Repo productCategory2Repo,
        IOrderRepo orderRepo, IOrder2Repo order2Repo,
        IOrderDetailRepo orderDetailRepo, IOrderDetail2Repo orderDetail2Repo
        )
    {
        _logger = logger;
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
            .RuleFor(p => p.Id, _ => customerId++)
            .RuleFor(p => p.Name, f => f.Name.FullName());

        var chunks = Convert.ToInt32(Math.Floor(count / Constants.BatchSize));
        var leftOver = Convert.ToInt32(count - chunks * Constants.BatchSize);

        List<Customer2>? customer2s;
        List<Customer>? customers;
        _logger.LogInformation("Insert customer: {Percent:P}", 0);
        for (var i = 0; i < chunks; i++)
        {
            customer2s = fakeCustomer.Generate(Constants.BatchSize);
            customers = customer2s.ConvertAll(p => (Customer)p);
            await _customer2Repo.InsertRangeAsync(customer2s);
            await _customerRepo.InsertRangeAsync(customers);
            _logger.LogInformation("Insert customer: {Percent:P}", (double) i / chunks);
        }
        
        customer2s = fakeCustomer.Generate(leftOver);
        customers = customer2s.ConvertAll(p => (Customer)p);
        await _customer2Repo.InsertRangeAsync(customer2s);
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

        List<ProductCategory2>? productCategory2s;
        List<ProductCategory>? productCategories;
        _logger.LogInformation("Insert product categories: {Percent:P}", 0);
        for (var i = 0; i < chunks; i++)
        {
            productCategory2s = fakeProductCategory.Generate(Constants.BatchSize);
            productCategories = productCategory2s.ConvertAll(p => (ProductCategory) p);
            await _productCategory2Repo.InsertRangeAsync(productCategory2s);
            await _productCategoryRepo.InsertRangeAsync(productCategories);
            _logger.LogInformation("Insert product categories: {Percent:P}", (double) i / chunks);
        }
        
        productCategory2s = fakeProductCategory.Generate(leftOver);
        productCategories = productCategory2s.ConvertAll(p => (ProductCategory) p);
        await _productCategory2Repo.InsertRangeAsync(productCategory2s);
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

        List<Product2>? product2s;
        List<Product>? products;
        _logger.LogInformation("Insert products: {Percent:P}", 0);
        for (var i = 0; i < chunks; i++)
        {
            product2s = fakeProduct.Generate(Constants.BatchSize);
            products = product2s.ConvertAll(p => (Product)p);
            await _product2Repo.InsertRangeAsync(product2s);
            await _productRepo.InsertRangeAsync(products);
            _logger.LogInformation("Insert products: {Percent:P}", (double) i / chunks);
        }
        
        product2s = fakeProduct.Generate(leftOver);
        products = product2s.ConvertAll(p => (Product)p);
        await _product2Repo.InsertRangeAsync(product2s);
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

        List<Order2>? order2s;
        List<Order>? orders;
        _logger.LogInformation("Insert orders: {Percent:P}", 0);
        for (var i = 0; i < chunks; i++)
        {
            order2s = fakeOrder.Generate(Constants.BatchSize);
            orders = order2s.ConvertAll(p => (Order)p);
            await _order2Repo.InsertRangeAsync(order2s);
            await _orderRepo.InsertRangeAsync(orders);
            _logger.LogInformation("Insert orders: {Percent:P}", (double) i / chunks);
        }
        
        order2s = fakeOrder.Generate(leftOver);
        orders = order2s.ConvertAll(p => (Order)p);
        await _order2Repo.InsertRangeAsync(order2s);
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
        var orderDetail2s = new List<OrderDetail2>();
        var orderDetails = new List<OrderDetail>();
        var faker = new Faker();
        for (orderId = 1; orderId <= count; orderId++)
        {
            orderDetail2s.AddRange(fakeOrderDetail.Generate(faker.Random.Int(1, 10)));

            if (orderId % Constants.BatchSize != 0) continue;

            orderDetails = orderDetail2s.ConvertAll(p => (OrderDetail) p);
            await _orderDetail2Repo.InsertRangeAsync(orderDetail2s);
            await _orderDetailRepo.InsertRangeAsync(orderDetails);
            orderDetail2s = new List<OrderDetail2>();
            _logger.LogInformation("Insert order details: {Percent:P}", orderId / count);
        }
        
        orderDetails = orderDetail2s.ConvertAll(p => (OrderDetail) p);
        await _orderDetail2Repo.InsertRangeAsync(orderDetail2s);
        await _orderDetailRepo.InsertRangeAsync(orderDetails);
        _logger.LogInformation("Insert order details: {Percent:P}", 1);
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
        await _orderDetailRepo.TruncateAsync();
        await _orderDetail2Repo.TruncateAsync();
    }
}