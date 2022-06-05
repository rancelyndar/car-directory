using System;
using System.Collections.Generic;
using System.Threading;
using CarDirectrory.Core;
using CarDirectrory.Core.Cars.Repositories;
using CarDirectrory.Core.Fines;
using CarDirectrory.Core.Fines.Repositories;
using CarDirectrory.Core.Statistics;
using CarDirectrory.Core.Statistics.Services;
using Moq;
using Xunit;

namespace CarDirectory.Core.Tests;

public class StatisticsServiceTests
{
    private readonly Mock<ICarRepository> _carRepositoryMock;
    private readonly Mock<IFineRepository> _fineRepositoryMock;
    private readonly IStatisticsService _statisticsService;

    public StatisticsServiceTests()
    {
        _carRepositoryMock = new Mock<ICarRepository>();
        _fineRepositoryMock = new Mock<IFineRepository>();

        _statisticsService = new StatisticsService(_carRepositoryMock.Object,
            _fineRepositoryMock.Object);
    }

    [Fact]
    public void GetStatisticsAsync_SuccessPath_ShouldReturnStatistics()
    {
        _carRepositoryMock
            .Setup(repository => repository.GetAllCarsAsync(CancellationToken.None))
            .ReturnsAsync(new List<Car>() {new Car()
            {
                Color = "",
                Model = "",
                ReleaseYear = 2000,
                StateNumber = "",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }});
        _fineRepositoryMock
            .Setup(repository => repository.GetAllFinesAsync(CancellationToken.None))
            .ReturnsAsync(new List<Fine>() { new Fine()
            {
                Id = "",
                IsPayed = false,
                Price = 100,
                ReceiptDate = DateTime.UtcNow,
                StateNumber = ""
            }});
        
        var statistics = _statisticsService.GetStatisticsAsync(CancellationToken.None);
        
        Assert.IsType<Statistics>(statistics.Result);
    }
}