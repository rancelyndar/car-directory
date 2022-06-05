using CarDirectrory.Core;

namespace CarDirectory.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly CarDirectoryContext _context;

    public UnitOfWork(CarDirectoryContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken token)
    {
        return _context.SaveChangesAsync(token);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
}