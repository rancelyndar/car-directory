using CarDirectrory.Core.Cars.Models;

namespace CarDirectrory.Core.Cars.Services;

public interface ICarService
{
    Task<IEnumerable<Car>> GetAllCarsAsync(CancellationToken token);
    Task CreateCarAsync(CreateCarModel model, CancellationToken token);
    Task DeleteCarAsync(string stateNumber, CancellationToken token);
    Task UpdateCarAsync(string stateNumber, UpdateCarModel model, CancellationToken token);
}