using CarDirectrory.Core.Cars.Services;
using CarDirectrory.Core.Fines.Services;
using CarDirectrory.Core.Statistics.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarDirectrory.Core;

public static class Registration
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IFineService, FineService>();
        services.AddScoped<IStatisticsService, StatisticsService>();
        services.AddFluentValidation().AddValidatorsFromAssembly(typeof(CarService).Assembly);
        return services;
    }
}