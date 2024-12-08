namespace SigmaSoftware.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);
    void Rollback();
    IGenericRepository<T> GenericRepository<T>() where T : class;

   

}