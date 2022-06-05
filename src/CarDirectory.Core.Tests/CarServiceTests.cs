using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CarDirectrory.Core;
using CarDirectrory.Core.Cars.Models;
using CarDirectrory.Core.Cars.Repositories;
using CarDirectrory.Core.Cars.Services;
using CarDirectrory.Core.Cars.Validators;
using FluentValidation;
using Moq;
using Xunit;

namespace CarDirectory.Core.Tests;

public class CarServiceTests
{
    private readonly ICarService _carService;
    private readonly Mock<ICarRepository> _carRepositoryMock;
    private readonly IValidator<CreateCarModel> _createCarValidator;
    private readonly IValidator<UpdateCarModel> _updateCarValidator;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CarServiceTests()
    {
        _carRepositoryMock = new Mock<ICarRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _createCarValidator = new CreateCarValidator();
        _updateCarValidator = new UpdateCarValidator();

        _carService = new CarService(
            _carRepositoryMock.Object,
            _createCarValidator,
            _updateCarValidator,
            _unitOfWorkMock.Object);
    }
    
    [Fact]
    public void GetAllCarsAsync_SuccessPath_ShouldReturnListOfCars()
    {
        _carRepositoryMock
            .Setup(repository => repository.GetAllCarsAsync(CancellationToken.None))
            .ReturnsAsync(new List<Car>());

        var cars = _carService.GetAllCarsAsync(CancellationToken.None);
        
        Assert.IsType<List<Car>>(cars.Result);
    }
    
    [Fact]
    public void CreateCarAsync_WithInvalidStateNumber_ShouldThrowException()
    {
        var car = new CreateCarModel() {StateNumber = "InvalidStateNumber"};

        var exception = Assert
            .ThrowsAsync<FluentValidation.ValidationException>(() =>
                _carService.CreateCarAsync(car, CancellationToken.None));
        
        Assert.Equal("Неверный формат номера",
            exception.Result.Errors.Select(x => $"{x.ErrorMessage}").First());
    }
    
    [Fact]
    public void CreateCarAsync_WithMinReleaseYear_ShouldThrowException()
    {
        var car = new CreateCarModel() {StateNumber = "АА111А11", ReleaseYear = int.MinValue};

        var exception = Assert
            .ThrowsAsync<FluentValidation.ValidationException>(() =>
                _carService.CreateCarAsync(car, CancellationToken.None));
        
        Assert.Equal("Год выпуска автомобиля не может быть меньше 1950",
            exception.Result.Errors.Select(x => $"{x.ErrorMessage}").First());
    }
    
    [Fact]
    public void CreateCarAsync_WithMaxReleaseYear_ShouldThrowException()
    {
        var car = new CreateCarModel() {StateNumber = "АА111А11", ReleaseYear = int.MaxValue};

        var exception = Assert
            .ThrowsAsync<FluentValidation.ValidationException>(() =>
                _carService.CreateCarAsync(car, CancellationToken.None));
        
        Assert.Equal("Год выпуска автомобиля превышает текущий год",
            exception.Result.Errors.Select(x => $"{x.ErrorMessage}").First());
    }
    
    [Fact]
    public void CreateCarAsync_CarAlreadyExists_ShouldThrowException()
    {
        _carRepositoryMock
            .Setup(repository => repository.CarExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);
        var car = new CreateCarModel() {StateNumber = "АА111А11", ReleaseYear = 2000, Color = "test", Model = "true"};

        var exception = Assert.
            ThrowsAsync<CarDirectrory.Core.ValidationException>(() => _carService.CreateCarAsync(car, CancellationToken.None));
        
        Assert.Equal("Автомобиль с данным номером уже существует", exception.Result.Message);
    }
    
    [Fact]
    public void CreateCarAsync_SuccessPath_ShouldCallRepositoryMethodOnce()
    {
        _carRepositoryMock
            .Setup(repository => repository.CreateCarAsync(It.IsAny<CreateCarModel>(), CancellationToken.None));
        var car = new CreateCarModel() {StateNumber = "АА111А11", Color = "test", Model = "test", ReleaseYear = 2000};

        _carService.CreateCarAsync(car, CancellationToken.None);
        
        _carRepositoryMock.Verify(mock => mock.CreateCarAsync(car, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact] public void DeleteCarAsync_CarDoesNotExist_ShouldThrowException()
    {
        _carRepositoryMock
            .Setup(repository => repository.CarExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(false);
        var stateNumber = "test";

        var exception = Assert.
            ThrowsAsync<CarDirectrory.Core.ValidationException>(() => _carService.DeleteCarAsync(stateNumber, CancellationToken.None));
        
        Assert.Equal("Автомобиля с данным номером не существует", exception.Result.Message);
    }
    
    [Fact]
    public void DeleteCarAsync_SuccessPath_ShouldCallRepositoryMethodOnce()
    {
        _carRepositoryMock
            .Setup(repository => repository.CarExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);
        var stateNumber = "test";

        _carService.DeleteCarAsync(stateNumber, CancellationToken.None);
        
        _carRepositoryMock.Verify(mock => mock.DeleteCarAsync(stateNumber, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
    
    [Fact] public void UpdateCarAsync_CarDoesNotExist_ShouldThrowException()
    {
        _carRepositoryMock
            .Setup(repository => repository.CarExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(false);
        var car = new UpdateCarModel() {ReleaseYear = 2000};
        var stateNumber = "test";

        var exception = Assert.
            ThrowsAsync<CarDirectrory.Core.ValidationException>(() => _carService.UpdateCarAsync(stateNumber,car, CancellationToken.None));
        
        Assert.Equal("Автомобиля с данным номером не существует", exception.Result.Message);
    }
    
    [Fact]
    public void UpdateCarAsync_WithMinReleaseYear_ShouldThrowException()
    {
        var car = new UpdateCarModel() {ReleaseYear = int.MinValue};
        var stateNumber = "test";

        var exception = Assert
            .ThrowsAsync<FluentValidation.ValidationException>(() =>
                _carService.UpdateCarAsync(stateNumber,car, CancellationToken.None));
        
        Assert.Equal("Год выпуска автомобиля не может быть меньше 1950",
            exception.Result.Errors.Select(x => $"{x.ErrorMessage}").First());
    }
    
    [Fact]
    public void UpdateCarAsync_WithMaxReleaseYear_ShouldThrowException()
    {
        var car = new UpdateCarModel() {ReleaseYear = int.MaxValue};
        var stateNumber = "test";

        var exception = Assert
            .ThrowsAsync<FluentValidation.ValidationException>(() =>
                _carService.UpdateCarAsync(stateNumber,car, CancellationToken.None));
        
        Assert.Equal("Год выпуска автомобиля превышает текущий год",
            exception.Result.Errors.Select(x => $"{x.ErrorMessage}").First());
    }
    
    [Fact]
    public void UpdateCarAsync_SuccessPath_ShouldCallRepositoryMethodOnce()
    {
        _carRepositoryMock
            .Setup(repository => repository.CarExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);
        var stateNumber = "test";
        var car = new UpdateCarModel() {ReleaseYear = 2000};

        _carService.UpdateCarAsync(stateNumber, car, CancellationToken.None);
        
        _carRepositoryMock.Verify(mock => mock.UpdateCarAsync(stateNumber, car, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}