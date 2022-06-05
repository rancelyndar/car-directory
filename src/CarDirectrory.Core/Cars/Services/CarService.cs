using CarDirectrory.Core.Cars.Models;
using CarDirectrory.Core.Cars.Repositories;
using FluentValidation;

namespace CarDirectrory.Core.Cars.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IValidator<CreateCarModel> _createCarValidator;
    private readonly IValidator<UpdateCarModel> _updateCarValidator;
    private readonly IUnitOfWork _unitOfWork;

    public CarService(ICarRepository carRepository,
        IValidator<CreateCarModel> createCarValidator,
        IValidator<UpdateCarModel> updateCarValidator,
        IUnitOfWork unitOfWork)
    {
        _carRepository = carRepository;
        _createCarValidator = createCarValidator;
        _updateCarValidator = updateCarValidator;
        _unitOfWork = unitOfWork;
    }

    private async Task ValidateAndThrow(string stateNumber, CancellationToken token)
    {
        if (!await _carRepository.CarExistsAsync(stateNumber, token))
        {
            throw new ValidationException("Автомобиля с данным номером не существует");
        }
    }
    
    private async Task ValidateAndThrowCreate(string stateNumber, CancellationToken token)
    {
        if (await _carRepository.CarExistsAsync(stateNumber, token))
        {
            throw new ValidationException("Автомобиль с данным номером уже существует");
        }
    }
    
    public async Task<IEnumerable<Car>> GetAllCarsAsync(CancellationToken token)
    {
        return await _carRepository.GetAllCarsAsync(token);
    }

    public async Task CreateCarAsync(CreateCarModel model, CancellationToken token)
    {
        await _createCarValidator.ValidateAndThrowAsync(model, token);
        await ValidateAndThrowCreate(model.StateNumber, token);

        await _carRepository.CreateCarAsync(model, token);
        await _unitOfWork.SaveChangesAsync(token);
    }

    public async Task DeleteCarAsync(string stateNumber, CancellationToken token)
    {
        await ValidateAndThrow(stateNumber, token);

        await _carRepository.DeleteCarAsync(stateNumber, token);
        await _unitOfWork.SaveChangesAsync(token);
    }

    public async Task UpdateCarAsync(string stateNumber, UpdateCarModel model, CancellationToken token)
    {
        await _updateCarValidator.ValidateAndThrowAsync(model, token);
        await ValidateAndThrow(stateNumber, token);

        await _carRepository.UpdateCarAsync(stateNumber, model, token);
        await _unitOfWork.SaveChangesAsync(token);
    }
}