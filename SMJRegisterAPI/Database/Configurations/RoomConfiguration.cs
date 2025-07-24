using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMJRegisterAPI.Entities;

namespace SMJRegisterAPI.Database.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Habitaciones");
        
        builder.HasKey(x => x.ID);
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .HasColumnName("NombreHabitacion");
        
        builder.Property(x=>x.Capacity)
            .HasColumnName("CapacidadMaxima");
        
        builder.HasMany(x => x.Campers)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}