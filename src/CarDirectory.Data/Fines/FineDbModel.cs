using CarDirectory.Data.Cars;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDirectory.Data.Fines;

public class FineDbModel
{
    public string Id { get; set; }
    public string StateNumber { get; set; }
    public double Price { get; set; }
    public DateTime ReceiptDate { get; set; }
    public bool IsPayed { get; set; }
    public CarDbModel Car { get; set; }
    
    internal class Map : IEntityTypeConfiguration<FineDbModel>
    {
        public void Configure(EntityTypeBuilder<FineDbModel> builder)
        {
            builder.ToTable("fine");


            builder.HasKey(it => it.Id);
            builder.HasOne(it => it.Car)
                .WithMany(it => it.Fines)
                .HasForeignKey(it => it.StateNumber);
        }
    }
}