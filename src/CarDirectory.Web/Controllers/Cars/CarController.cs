using CarDirectory.Web.Controllers.Dto;
using CarDirectrory.Core;
using CarDirectrory.Core.Cars.Models;
using CarDirectrory.Core.Cars.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarDirectory.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CarController
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }
    
    private CarDto ConvertToCarDto(Car car)
    {
        return new CarDto()
        {
            StateNumber = car.StateNumber,
            Color = car.Color,
            Model = car.Model,
            ReleaseYear = car.ReleaseYear
        };
    }
    
    [HttpGet]
    public async Task<IEnumerable<CarDto>> GetCars(CancellationToken token,
        string? stateNumber,
        string? color,
        string? model,
        int? minReleaseYear,
        int? maxReleaseYear)
    {
        var cars = (await _carService.GetAllCarsAsync(token)).Select(ConvertToCarDto);
        
        if (stateNumber != null)
        {
            cars = cars.Where(car => car.StateNumber == stateNumber);
        }
        if (color != null)
        {
            cars = cars.Where(car => car.Color == color);
        }
        if (model != null)
        {
            cars = cars.Where(car => car.Model == model);
        }
        if (minReleaseYear != null)
        {
            cars = cars.Where(car => car.ReleaseYear >= minReleaseYear);
        }
        if (maxReleaseYear != null)
        {
            cars = cars.Where(car => car.ReleaseYear <= maxReleaseYear);
        }

        return cars;
    }
    
    [HttpPost]
    public Task CreateCar(CarDto car, CancellationToken token)
    {
        return _carService.CreateCarAsync(new CreateCarModel()
        {
            StateNumber = car.StateNumber,
            Color = car.Color,
            Model = car.Model,
            ReleaseYear = car.ReleaseYear
        }, token);
    } 
    
    [HttpDelete("{stateNumber}")]
    public Task DeleteCar(string stateNumber, CancellationToken token)
    {
        return _carService.DeleteCarAsync(stateNumber, token);
    }

    [HttpPatch("{stateNumber}")]
    public Task UpdateCar(string stateNumber, UpdateCarDto car, CancellationToken token)
    {
        return _carService.UpdateCarAsync(stateNumber,new UpdateCarModel()
        {
            Color = car.Color,
            Model = car.Model,
            ReleaseYear = car.ReleaseYear
        }, token);
    }
}