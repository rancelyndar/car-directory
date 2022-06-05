using CarDirectrory.Core.Cars.Repositories;
using CarDirectrory.Core.Fines.Repositories;

namespace CarDirectrory.Core.Statistics.Services;

public class StatisticsService : IStatisticsService
{
    private readonly ICarRepository _carRepository;
    private readonly IFineRepository _fineRepository;

    public StatisticsService(ICarRepository carRepository,
        IFineRepository fineRepository)
    {
        _carRepository = carRepository;
        _fineRepository = fineRepository;
    }
    
    public async Task<Statistics> GetStatisticsAsync(CancellationToken token)
    {
        var cars = await _carRepository.GetAllCarsAsync(token);
        var fines = await _fineRepository.GetAllFinesAsync(token);

        return new Statistics()
        {
            AmountOfCars = cars.Count(),
            AmountOfFinedCars = fines.GroupBy(fine => fine.StateNumber).Count(),
            FirstEntryDate = cars.Min(car => car.CreatedAt),
            LastEntryDate = cars.Max(car => car.CreatedAt)
        };
    }
}