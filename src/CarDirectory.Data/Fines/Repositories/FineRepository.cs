using CarDirectrory.Core.Fines;
using CarDirectrory.Core.Fines.Models;
using CarDirectrory.Core.Fines.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarDirectory.Data.Fines.Repositories;

public class FineRepository : IFineRepository
{
    private readonly CarDirectoryContext _context;

    public FineRepository(CarDirectoryContext context)
    {
        _context = context;
    }

    public async Task<Fine> GetFineByIdAsync(string id, CancellationToken token)
    {
        var fine = await _context.Fines.AsNoTracking().FirstOrDefaultAsync(fine => fine.Id == id, token);

        return new Fine()
        {
            Id = fine.Id,
            IsPayed = fine.IsPayed,
            ReceiptDate = fine.ReceiptDate,
            Price = fine.Price,
            StateNumber = fine.StateNumber
        };
    }

    public async Task<IEnumerable<Fine>> GetAllFinesAsync(CancellationToken token)
    {
        return await _context.Fines
            .AsNoTracking()
            .Select(fine => new Fine()
            {
                Id = fine.Id,
                StateNumber = fine.StateNumber,
                Price = fine.Price,
                ReceiptDate = fine.ReceiptDate,
                IsPayed = fine.IsPayed
            }).ToListAsync(token);
    }

    public async Task CreateFineAsync(CreateFineModel model, CancellationToken token)
    {
        var newFine = new FineDbModel()
        {
            Id = Guid.NewGuid().ToString(),
            StateNumber = model.StateNumber,
            Price = model.Price,
            ReceiptDate = DateTime.UtcNow,
            IsPayed = false
        };
            
        await _context.Fines.AddAsync(newFine, token);
    }

    public async Task<IEnumerable<Fine>> GetCarFinesAsync(string stateNumber, CancellationToken token)
    {
        return await _context.Fines
            .AsNoTracking()
            .Where(fine => fine.StateNumber == stateNumber)
            .Select(fine => new Fine()
            {
                Id = fine.Id,
                StateNumber = fine.StateNumber,
                Price = fine.Price,
                ReceiptDate = fine.ReceiptDate,
                IsPayed = fine.IsPayed
            }).ToListAsync(token);
    }

    public async Task PayFineAsync(PayFineModel model, CancellationToken token)
    {
        var fine = await _context.Fines.FirstOrDefaultAsync(fine => fine.Id == model.Id, token);
        
        fine.IsPayed = true;
    }

    public async Task<bool> FineExistsAsync(string id, CancellationToken token)
    {
        return await _context.Fines.FirstOrDefaultAsync(fine => fine.Id == id, token) != null;
    }
}