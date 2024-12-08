using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SigmaSoftware.Application.Common.Interfaces;
using DbContext = SigmaSoftware.Infrastructure.Persistence.DbContext;

namespace SigmaSoftware.Infrastructure.Services;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DbContext _dbContext;
    private readonly DbSet<T> _dbSet;
    public T GetById(object id)
    {
        return _dbSet.Find(id);
    }
    public GenericRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<T> GetByIdAsync(Guid? id)
    {
        return await _dbSet.FindAsync(id);
    }

    public IQueryable<T> GetAllNoTracking()
    {
        return _dbSet.AsQueryable().AsNoTracking();
    }

    public async Task<EntityEntry<T>> InsertAsync(T entity)
    {
        return await _dbSet.AddAsync(entity);
    }

    public Task  DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public void UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
    }
    
    public async Task AddRangeAsync(IEnumerable<T> entity)
    {
        await _dbSet.AddRangeAsync(entity);
    }

    public Task RemoveRange(IEnumerable<T> entity)
    {
        _dbSet.RemoveRange(entity);
        return Task.CompletedTask;
    }

    public async Task<Guid> InsertAndGetAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        //Returns primaryKey value
        return (Guid)entity.GetType().GetProperty("Id").GetValue(entity, null);
    }

    public async Task<Guid> UpdateAndGetAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        //Returns primaryKey value
        return (Guid)entity.GetType().GetProperty("Id").GetValue(entity, null);
    }
    
    public Task UpdateRangeAsync(IEnumerable<T> entity)
    {
        _dbSet.UpdateRange(entity);
        return Task.CompletedTask;
    }
    
    public T? SingleOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> predicate)  
    {  
        return _dbSet.Where(predicate).SingleOrDefault();  
    } 
    
    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)  
    {  
        return await _dbSet.Where(predicate).FirstOrDefaultAsync();  
    }

    public async Task<IQueryable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }
    
    public IQueryable<T> GetAll()
    {
        return _dbSet.AsQueryable();
    }
    
}