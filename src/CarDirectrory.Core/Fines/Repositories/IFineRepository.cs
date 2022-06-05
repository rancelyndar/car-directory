using CarDirectrory.Core.Fines.Models;

namespace CarDirectrory.Core.Fines.Repositories;

public interface IFineRepository
{
    Task<Fine> GetFineByIdAsync(string id, CancellationToken token);
    Task<IEnumerable<Fine>> GetAllFinesAsync(CancellationToken token);
    Task CreateFineAsync(CreateFineModel model, CancellationToken token);
    Task<IEnumerable<Fine>> GetCarFinesAsync(string stateNumber, CancellationToken token);
    Task PayFineAsync(PayFineModel model, CancellationToken token);
    Task<bool> FineExistsAsync(string id, CancellationToken token);
}