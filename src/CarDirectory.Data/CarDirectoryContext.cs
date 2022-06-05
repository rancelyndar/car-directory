using CarDirectory.Data.Cars;
using CarDirectory.Data.Fines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CarDirectory.Data;

public class CarDirectoryContext : DbContext
{
    public DbSet<CarDbModel> Cars { get; set; }
    public DbSet<FineDbModel> Fines { get; set; }
    
    public CarDirectoryContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarDirectoryContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseNpgsql().UseSnakeCaseNamingConvention();

    public class Factory : IDesignTimeDbContextFactory<CarDirectoryContext>
    {
        public CarDirectoryContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../CarDirectory.Web"))
                .AddJsonFile("appsettings.json")
                .Build();
            
            var connectionString = config.GetConnectionString("DbConnectionString");
            
            var options = new DbContextOptionsBuilder()
                .UseNpgsql(connectionString)
                .Options;

            return new CarDirectoryContext(options);
        }
    }
}