using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CarDirectrory.Core;
using CarDirectrory.Core.Cars.Repositories;
using CarDirectrory.Core.Fines;
using CarDirectrory.Core.Fines.Models;
using CarDirectrory.Core.Fines.Repositories;
using CarDirectrory.Core.Fines.Services;
using CarDirectrory.Core.Fines.Validators;
using FluentValidation;
using Moq;
using Xunit;

namespace CarDirectory.Core.Tests;

public class FineServiceTests
{
    private readonly Mock<IFineRepository> _fineRepositoryMock;
    private readonly Mock<ICarRepository> _carRepositoryMock;
    private readonly IValidator<CreateFineModel> _createFineValidator;
    private readonly IValidator<PayFineModel> _payFineValidator;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IFineService _fineService;

    public FineServiceTests()
    {
        _fineRepositoryMock = new Mock<IFineRepository>();
        _carRepositoryMock = new Mock<ICarRepository>();
        _createFineValidator = new CreateFineValidator();
        _payFineValidator = new PayFineValidator();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _fineService = new FineService(_fineRepositoryMock.Object,
            _carRepositoryMock.Object,
            _createFineValidator,
            _payFineValidator,
            _unitOfWorkMock.Object);
    }
    
    [Fact]
    public void CreateFineAsync_CarDoesNotExist_ShouldThrowException()
    {
        _carRepositoryMock
            .Setup(repository => repository.CarExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(false);
        var fine = new CreateFineModel() {StateNumber = "test", Price = 100};

        var exception = Assert.
            ThrowsAsync<CarDirectrory.Core.ValidationException>(() => _fineService.CreateFineAsync(fine, CancellationToken.None));
        
        Assert.Equal("Автомобиля с данным номером не существует", exception.Result.Message);
    }
    
    [Fact]
    public void CreateFineAsync_WithNegativePrice_ShouldThrowException()
    {
        _carRepositoryMock
            .Setup(repository => repository.CarExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);
        var fine = new CreateFineModel() {StateNumber = "test", Price = int.MinValue};

        var exception = Assert
            .ThrowsAsync<FluentValidation.ValidationException>(() =>
                _fineService.CreateFineAsync(fine, CancellationToken.None));
        
        Assert.Equal("Размер штрафа не может быть меньше или равен нулю",
            exception.Result.Errors.Select(x => $"{x.ErrorMessage}").First());
    }
    
    [Fact]
    public void CreateFineAsync_SuccessPath_ShouldCallRepositoryMethodOnce()
    {
        _carRepositoryMock
            .Setup(repository => repository.CarExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);
        var fine = new CreateFineModel() {StateNumber = "test", Price = 100};

        _fineService.CreateFineAsync(fine, CancellationToken.None);
        
        _fineRepositoryMock.Verify(mock => mock.CreateFineAsync(fine, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public void GetCarFinesAsync_CarDoesNotExist_ShouldThrowException()
    {
        _carRepositoryMock
            .Setup(repository => repository.CarExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(false);
        var stateNumber = "test";

        var exception = Assert.
            ThrowsAsync<CarDirectrory.Core.ValidationException>(() => _fineService.GetCarFinesAsync(stateNumber, CancellationToken.None));
        
        Assert.Equal("Автомобиля с данным номером не существует", exception.Result.Message);
    }
    
    [Fact]
    public void GetCarFinesAsync_SuccessPath_ShouldReturnListOfFines()
    {
        _carRepositoryMock
            .Setup(repository => repository.CarExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);
        _fineRepositoryMock
            .Setup(repository => repository.GetCarFinesAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new List<Fine>());
        var stateNumber = "test";
        
        var fines = _fineService.GetCarFinesAsync(stateNumber, CancellationToken.None);
        
        Assert.IsType<List<Fine>>(fines.Result);
    }

    [Fact]
    public void PayFineAsync_FineDoesNotExist_ShouldThrowException()
    {
        _fineRepositoryMock
            .Setup(repository => repository.FineExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(false);
        var id = "id";

        var exception = Assert.
            ThrowsAsync<CarDirectrory.Core.ValidationException>(() => _fineService.PayFineAsync(id, CancellationToken.None));
        
        Assert.Equal("Штрафа с данным id не существует", exception.Result.Message);
    }
    
    [Fact]
    public void PayFineAsync_FineIsAlreadyPayed_ShouldThrowException()
    {
        _fineRepositoryMock
            .Setup(repository => repository.FineExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);
        _fineRepositoryMock
            .Setup(repository => repository.GetFineByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Fine() {Id = "id", IsPayed = true});
        var payFineModel = new PayFineModel() {Id = "id", IsPayed = true};

        var exception = Assert
            .ThrowsAsync<FluentValidation.ValidationException>(() =>
                _fineService.PayFineAsync(payFineModel.Id, CancellationToken.None));
        
        Assert.Equal("Штраф уже оплачен",
            exception.Result.Errors.Select(x => $"{x.ErrorMessage}").First());
    }
    
    [Fact]
    public void PayFineAsync_SuccessPath_ShouldCallRepositoryMethodOnce()
    {
        _fineRepositoryMock
            .Setup(repository => repository.FineExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);
        _fineRepositoryMock
            .Setup(repository => repository.GetFineByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Fine() {Id = "id", IsPayed = false});
        var payFineModel = new PayFineModel() {Id = "id", IsPayed = false};

        _fineService.PayFineAsync(payFineModel.Id, CancellationToken.None);
        
        _fineRepositoryMock.Verify(mock => mock.PayFineAsync(It.IsAny<PayFineModel>(), CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}