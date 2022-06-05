using CarDirectrory.Core;
using CarDirectrory.Core.Cars.Models;
using CarDirectrory.Core.Cars.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarDirectory.Data.Cars.Repositories;

public class CarRepository : ICarRepository
{
    private readonly CarDirectoryContext _context;

    public CarRepository(CarDirectoryContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Car>> GetAllCarsAsync(CancellationToken token)
    {
        return await _context.Cars
            .AsNoTracking()
            .Select(car => new Car()
            {
                StateNumber = car.StateNumber,
                Color = car.Color,
                Model = car.Model,
                ReleaseYear = car.ReleaseYear,
                CreatedAt = car.CreatedAt,
                UpdatedAt = car.UpdatedAt
            }).ToListAsync(token);
    }

    public async Task CreateCarAsync(CreateCarModel model, CancellationToken token)
    {
        var newCar = new CarDbModel()
        {
            StateNumber = model.StateNumber,
            Color = model.Color,
            Model = model.Model,
            ReleaseYear = model.ReleaseYear,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Cars.AddAsync(newCar, token);
    }

    public async Task DeleteCarAsync(string stateNumber, CancellationToken token)
    {
        var carToDelete = await _context.Cars.FirstOrDefaultAsync(car => car.StateNumber == stateNumber, token);

        _context.Cars.Remove(carToDelete);
    }

    public async Task<bool> CarExistsAsync(string stateNumber, CancellationToken token)
    {
        return await _context.Cars.AsNoTracking().FirstOrDefaultAsync(car => car.StateNumber == stateNumber, token) != null;
    }

    public async Task UpdateCarAsync(string stateNumber, UpdateCarModel model, CancellationToken token)
    {
        var carToUpdate = await _context.Cars.FirstOrDefaultAsync(it => it.StateNumber == stateNumber, token);
        
        carToUpdate.Model = model.Model;
        carToUpdate.Color = model.Color;
        carToUpdate.ReleaseYear = model.ReleaseYear;
        carToUpdate.UpdatedAt = DateTime.UtcNow;
    }
}