using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMJRegisterAPI.Entities;

namespace SMJRegisterAPI.Database.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Habitaciones");
        
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .HasColumnName("NombreHabitacion");
        
        builder.Property(x=>x.Capacity)
            .HasColumnName("CapacidadMaxima");
        
    }
}