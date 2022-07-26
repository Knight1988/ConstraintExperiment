using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories;

public abstract class BaseRepo<T> where T : class
{
    private readonly DbSet<T> _dbSet;
    private readonly string _tableName;
    private readonly DbContext _context;

    protected BaseRepo(DbContext context, DbSet<T> dbSet, string tableName)
    {
        _dbSet = dbSet;
        _context = context;
        _tableName = tableName;
    }
    
    public async Task InsertRangeAsync(IEnumerable<T> objs)
    {
        await _dbSet.AddRangeAsync(objs);
        await _context.SaveChangesAsync();
    }

    public async Task TruncateAsync()
    {
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM " + _tableName);
    }
}