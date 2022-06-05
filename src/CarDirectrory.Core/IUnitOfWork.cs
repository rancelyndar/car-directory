namespace CarDirectrory.Core;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken token);
    int SaveChanges();
}