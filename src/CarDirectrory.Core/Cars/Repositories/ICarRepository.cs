using CarDirectrory.Core.Cars.Models;

namespace CarDirectrory.Core.Cars.Repositories;

public interface ICarRepository
{
    Task<IEnumerable<Car>> GetAllCarsAsync(CancellationToken token);
    Task CreateCarAsync(CreateCarModel model, CancellationToken token);
    Task DeleteCarAsync(string stateNumber, CancellationToken token);
    Task<bool> CarExistsAsync(string stateNumber, CancellationToken token);
    Task UpdateCarAsync(string stateNumber, UpdateCarModel model, CancellationToken token);
}