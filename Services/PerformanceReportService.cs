using Bogus;
using ConstraintExperiment.Commons;
using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Interfaces.Constraint;
using ConstraintExperiment.Interfaces.NonConstraint;
using ConstraintExperiment.Models;
using ConstraintExperiment.Models.Constraint;

namespace ConstraintExperiment.Services;

public class PerformanceReportService : IPerformanceReportService
{
    private readonly IConfiguration _configuration;
    private readonly IReportMonthRepo _reportMonthRepo;
    private readonly IReportMonth2Repo _reportMonth2Repo;
    private readonly IReportYearlyRepo _reportYearlyRepo;
    private readonly IReportYearly2Repo _reportYearly2Repo;
    private readonly IProductRepo _productRepo;
    private readonly IProduct2Repo _product2Repo;
    private readonly ICustomerRepo _customerRepo;
    private readonly ICustomer2Repo _customer2Repo;
    private readonly IFakerService _fakerService;

    public PerformanceReportService(IConfiguration configuration, IFakerService fakerService,
        IReportMonthRepo reportMonthRepo, IReportMonth2Repo reportMonth2Repo,
        IReportYearlyRepo reportYearlyRepo, IReportYearly2Repo reportYearly2Repo,
        IProductRepo productRepo, IProduct2Repo product2Repo,
        ICustomerRepo customerRepo, ICustomer2Repo customer2Repo)
    {
        _configuration = configuration;
        _fakerService = fakerService;
        _reportMonthRepo = reportMonthRepo;
        _reportMonth2Repo = reportMonth2Repo;
        _reportYearlyRepo = reportYearlyRepo;
        _reportYearly2Repo = reportYearly2Repo;
        _productRepo = productRepo;
        _product2Repo = product2Repo;
        _customerRepo = customerRepo;
        _customer2Repo = customer2Repo;
    }

    public async Task<string> WriteToFileAsync(IEnumerable<PerformanceReport> reports)
    {
        var provider = _configuration.GetValue<string>("Provider");
        var fileName = $"TestReport-{provider}.md";
        await using StreamWriter file = new(fileName);
        await file.WriteLineAsync("## Performance Test Report");
        await file.WriteLineAsync(string.Empty);
        await file.WriteLineAsync($"Provider: {provider}");
        await file.WriteLineAsync(string.Empty);
        await file.WriteLineAsync($"Test time: {DateTime.Now:yyyy/MM/dd HH:mm:ss}");
        await file.WriteLineAsync(string.Empty);
        await file.WriteLineAsync($"Customer: {_configuration.GetValue<int>("Fakes:CustomerCount"):N0}");
        await file.WriteLineAsync(string.Empty);
        await file.WriteLineAsync($"Product: {_configuration.GetValue<int>("Fakes:ProductCount"):N0}");
        await file.WriteLineAsync(string.Empty);
        await file.WriteLineAsync($"Order: {_configuration.GetValue<int>("Fakes:OrderCount"):N0}");
        await file.WriteLineAsync(string.Empty);
        await file.WriteLineAsync("Bellow is test performance report. Lower is better");
        await file.WriteLineAsync(string.Empty);
        await file.WriteLineAsync("|Test case|Constraint|Non Constraint|");
        await file.WriteLineAsync("|--|--|--|");

        foreach (var report in reports)
        {
            var totalConstraint = 0d;
            var totalNonConstraint = 0d;
            var repeatTime = _configuration.GetValue<int>("TestRepeatTimes");
            for (var i = 0; i < repeatTime; i++)
            {
                totalConstraint += report.ConstraintTimes[i];
                totalNonConstraint += report.NonConstraintTimes[i];
                await file.WriteLineAsync(
                    $"|{report.Content}|{report.ConstraintTimes[i]}ms|{report.NonConstraintTimes[i]}ms|");
            }

            var avgConstraint = totalConstraint / repeatTime;
            var avgNonConstraint = totalNonConstraint / repeatTime;
            await file.WriteLineAsync($"|Avg|{avgConstraint}ms|{avgNonConstraint}ms|");
        }

        await file.FlushAsync();
        return Path.GetFullPath(fileName);
    }

    private async Task<PerformanceReport> PerformanceTest(string content,
        Func<Task> nonConstraintFunc,
        Func<Task> constraintFunc)
    {
        var report = new PerformanceReport
        {
            Content = content,
            ConstraintTimes = new List<long>(),
            NonConstraintTimes = new List<long>()
        };

        var repeatTimes = _configuration.GetValue<int>("TestRepeatTimes");

        for (var i = 0; i <= repeatTimes; i++)
        {
            var elapsedTime = await nonConstraintFunc().GetElapsedTimeAsync();
            // skip first result
            if (i != 0) report.NonConstraintTimes.Add(elapsedTime);
        }

        for (var i = 0; i <= repeatTimes; i++)
        {
            var elapsedTime = await constraintFunc().GetElapsedTimeAsync();
            // skip first result
            if (i != 0) report.ConstraintTimes.Add(elapsedTime);
        }

        return report;
    }

    public async Task<PerformanceReport> RevenueLastMonthAsync()
    {
        return await PerformanceTest("Get revenue last month", 
            () => _reportMonthRepo.RevenueMonthlyAsync(3),
            () => _reportMonth2Repo.RevenueMonthlyAsync(3));
    }

    public async Task<PerformanceReport> RevenueInYearAsync()
    {
        var thisYear = DateTime.Now.Year;
        return await PerformanceTest("Get revenue this year", 
            () => _reportYearlyRepo.RevenueInYearAsync(thisYear),
            () => _reportYearly2Repo.RevenueInYearAsync(thisYear));
    }

    public async Task<PerformanceReport> BestSellerProductInYearAsync()
    {
        var thisYear = DateTime.Now.Year;
        return await PerformanceTest("Best seller product this year", 
            () => _reportYearlyRepo.BestSellerInYearAsync(thisYear, 10),
            () => _reportYearly2Repo.BestSellerInYearAsync(thisYear, 10));
    }

    public async Task<PerformanceReport> TopCustomerInYearAsync()
    {
        var thisYear = DateTime.Now.Year;
        return await PerformanceTest("Top customer this year", 
            () => _reportYearlyRepo.TopCustomerInYearAsync(thisYear, 10),
            () => _reportYearly2Repo.TopCustomerInYearAsync(thisYear, 10));
    }

    public async Task<PerformanceReport> SearchProductAsync()
    {
        return await PerformanceTest("Search Product", 
            () => _productRepo.SearchAsync("Chicken", 0, 20),
            () => _product2Repo.SearchAsync("Chicken", 0, 20));
    }

    public async Task<PerformanceReport> InsertCustomerAsync()
    {
        return await PerformanceTest("Insert customer", 
            () =>
            {
                var customers = _fakerService.GenerateCustomer(1);
                return _customerRepo.InsertAsync(customers[0]);
            },
            () =>
            {
                var customers = _fakerService.GenerateCustomer(1);
                return _customer2Repo.InsertAsync(customers[0]);
            });
    }

    public async Task<PerformanceReport> UpdateCustomerAsync()
    {
        return await PerformanceTest("Update customer", async () =>
            {
                var customer = await _customerRepo.GetByIdAsync(1);
                await _customerRepo.UpdateAsync(customer);
            }, async () =>
            {
                var customer = await _customer2Repo.GetByIdAsync(1);
                await _customer2Repo.UpdateAsync(customer);
            });
    }

    public async Task<PerformanceReport> DeleteCustomerAsync()
    {
        var i = 1;
        return await PerformanceTest("Delete customer", async () =>
            {
                var customer = await _customerRepo.GetByIdAsync(i++);
                await _customerRepo.DeleteAsync(customer);
            }, async () =>
            {
                var customer = await _customer2Repo.GetByIdAsync(i++);
                await _customer2Repo.DeleteAsync(customer);
            });
    }
}