namespace ConstraintExperiment.Interfaces;

public interface IBaseRepo<T> where T: IBaseModel
{
    Task<T?> GetByIdAsync(int id);
    Task InsertAsync(T obj);
    Task InsertRangeAsync(IList<T> objs);
    Task UpdateAsync(T obj);
    Task DeleteAsync(T obj);
}