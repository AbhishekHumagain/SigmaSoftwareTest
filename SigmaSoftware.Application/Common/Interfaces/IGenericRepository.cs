using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SigmaSoftware.Application.Common.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid? id);
    IQueryable<T> GetAllNoTracking();
    Task<IQueryable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
    Task<EntityEntry<T>> InsertAsync(T entity);
    Task DeleteAsync(T entity);
    void UpdateAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entity);
    Task RemoveRange(IEnumerable<T> entity);
    Task<Guid> InsertAndGetAsync(T entity);
    Task<Guid> UpdateAndGetAsync(T entity, CancellationToken cancellationToken);
    Task UpdateRangeAsync(IEnumerable<T> entity);
    T? SingleOrDefault(Expression<Func<T, bool>> predicate);  
  
    /// <summary>  
    /// First the or default.  
    /// </summary>  
    /// <returns></returns>  
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    IQueryable<T> GetAll();
}