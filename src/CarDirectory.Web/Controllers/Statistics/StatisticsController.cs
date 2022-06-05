using CarDirectrory.Core.Statistics.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarDirectory.Web.Controllers.Statistics;

[ApiController]
[Route("[controller]")]
public class StatisticsController
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet]
    public async Task<CarDirectrory.Core.Statistics.Statistics> GetStatistics(CancellationToken token)
    {
        return await _statisticsService.GetStatisticsAsync(token);
    }
}