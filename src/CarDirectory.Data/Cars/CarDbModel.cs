using CarDirectory.Data.Fines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDirectory.Data.Cars;

public class CarDbModel
{
    public string StateNumber { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public int ReleaseYear { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<FineDbModel> Fines { get; set; }
    
    internal class Map : IEntityTypeConfiguration<CarDbModel>
    {
        public void Configure(EntityTypeBuilder<CarDbModel> builder)
        {
            builder.ToTable("car");

            builder.HasKey(it => it.StateNumber);
        }
    }
}