namespace ConstraintExperiment.Interfaces;

public interface IDataGenerateService
{
    Task MigrateDatabaseAsync();
    Task FakeCustomerAsync();
    Task FakeProductCategoryAsync();
    Task FakeProductAsync();
    Task FakeOrderAsync();
    Task FakeOrderDetailAsync();
    Task TearDownDatabaseAsync();
}