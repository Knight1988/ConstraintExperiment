namespace ConstraintExperiment.Interfaces;

public interface IDataGenerateService
{
    Task FakeCustomerAsync();
    Task FakeProductAsync();
    Task TruncateDatabaseAsync();
}