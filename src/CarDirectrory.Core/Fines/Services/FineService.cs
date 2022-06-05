using CarDirectrory.Core.Cars.Repositories;
using CarDirectrory.Core.Fines.Models;
using CarDirectrory.Core.Fines.Repositories;
using FluentValidation;

namespace CarDirectrory.Core.Fines.Services;

public class FineService : IFineService
{
    private readonly IFineRepository _fineRepository;
    private readonly ICarRepository _carRepository;
    private readonly IValidator<CreateFineModel> _createFineValidator;
    private readonly IValidator<PayFineModel> _payFineValidator;
    private readonly IUnitOfWork _unitOfWork;

    public FineService(IFineRepository fineRepository,
        ICarRepository carRepository,
        IValidator<CreateFineModel> createFineValidator,
        IValidator<PayFineModel> payFineValidator,
        IUnitOfWork unitOfWork)
    {
        _fineRepository = fineRepository;
        _carRepository = carRepository;
        _createFineValidator = createFineValidator;
        _payFineValidator = payFineValidator;
        _unitOfWork = unitOfWork;
    }

    private async Task ValidateAndThrowCar(string stateNumber, CancellationToken token)
    {
        if (!await _carRepository.CarExistsAsync(stateNumber, token))
        {
            throw new ValidationException("Автомобиля с данным номером не существует");
        }
    }
    
    private async Task ValidateAndThrowFine(string id, CancellationToken token)
    {
        if (!await _fineRepository.FineExistsAsync(id, token))
        {
            throw new ValidationException("Штрафа с данным id не существует");
        }
    }
    
    public async Task CreateFineAsync(CreateFineModel model, CancellationToken token)
    {
        await ValidateAndThrowCar(model.StateNumber, token);
        await _createFineValidator.ValidateAndThrowAsync(model, token);

        await _fineRepository.CreateFineAsync(model, token);
        await _unitOfWork.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<Fine>> GetCarFinesAsync(string stateNumber, CancellationToken token)
    {
        await ValidateAndThrowCar(stateNumber, token);

        return await _fineRepository.GetCarFinesAsync(stateNumber, token);
    }

    public async Task PayFineAsync(string id, CancellationToken token)
    {
        await ValidateAndThrowFine(id, token);
        var fine = await _fineRepository.GetFineByIdAsync(id, token);
        var model = new PayFineModel()
        {
            Id = fine.Id,
            IsPayed = fine.IsPayed
        };
        await _payFineValidator.ValidateAndThrowAsync(model, token);

        await _fineRepository.PayFineAsync(model, token);
        await _unitOfWork.SaveChangesAsync(token);
    }
}