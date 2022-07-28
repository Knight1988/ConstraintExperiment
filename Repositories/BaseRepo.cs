using ConstraintExperiment.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories;

public abstract class BaseRepo<T> : IBaseRepo<T> where T : class, IBaseModel
{
    private readonly DbSet<T> _dbSet;
    private readonly string _tableName;
    private readonly BaseContext _context;

    protected BaseRepo(BaseContext context, DbSet<T> dbSet, string tableName)
    {
        _dbSet = dbSet;
        _context = context;
        _tableName = tableName;
    }
    
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.SingleOrDefaultAsync(p => p.Id == id);
    }

    public virtual async Task InsertAsync(T obj)
    {
        await _context.AddAsync(obj);
        await _context.SaveChangesAsync();
    }
    
    public virtual async Task InsertRangeAsync(IList<T> objs)
    {
        if (BaseContext.IsMySql)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            await _context.AddRangeAsync(objs);
            await _context.SaveChangesAsync();
        }
        else
        {
            await _context.BulkInsertAsync(objs);
        }
    }

    public virtual async Task UpdateAsync(T obj)
    {
        _context.Update(obj);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T obj)
    {
        _context.Remove(obj);
        await _context.SaveChangesAsync();
    }
}