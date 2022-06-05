namespace CarDirectrory.Core.Statistics.Services;

public interface IStatisticsService
{
    Task<Statistics> GetStatisticsAsync(CancellationToken token);
}