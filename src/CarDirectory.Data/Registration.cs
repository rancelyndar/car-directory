using CarDirectory.Data.Cars.Repositories;
using CarDirectory.Data.Fines.Repositories;
using CarDirectrory.Core;
using CarDirectrory.Core.Cars.Repositories;
using CarDirectrory.Core.Fines.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarDirectory.Data;

public static class Registration
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFineRepository, FineRepository>();
        services.AddDbContext<CarDirectoryContext>(options => options.UseNpgsql(
            configuration["ConnectionStrings:DbConnectionString"]));
        return services;
    }
}