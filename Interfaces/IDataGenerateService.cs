namespace ConstraintExperiment.Interfaces;

public interface IDataGenerateService
{
    Task FakeCustomerAsync();
    Task FakeProductCategoryAsync();
    Task FakeProductAsync();
    Task TruncateDatabaseAsync();
}