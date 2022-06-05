using CarDirectrory.Core.Fines.Models;

namespace CarDirectrory.Core.Fines.Services;

public interface IFineService
{
    Task CreateFineAsync(CreateFineModel model, CancellationToken token);
    Task<IEnumerable<Fine>> GetCarFinesAsync(string stateNumber, CancellationToken token);
    Task PayFineAsync(string id, CancellationToken token);
}