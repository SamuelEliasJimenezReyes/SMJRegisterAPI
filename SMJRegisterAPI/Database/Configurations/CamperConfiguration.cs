using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMJRegisterAPI.Entities;

namespace SMJRegisterAPI.Database.Configurations;

public class CamperConfiguration : IEntityTypeConfiguration<Camper>
{
    public void Configure(EntityTypeBuilder<Camper> builder)
    {
        builder.ToTable("Campistas");
        
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.HasKey(x => x.ID);
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .HasColumnName("Nombre");
        
        builder.Property(x => x.DocumentNumber)
            .IsRequired()
            .HasMaxLength(11)
            .HasColumnName("Cedula");

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .HasColumnName("Apellido");
        
        builder.Property(x => x.PaidAmount)
            .HasColumnName("CantidadPaga")
            .HasColumnType("decimal(18,2)");
        
        builder.Property(x => x.IsGrant)
            .HasColumnName("Becado");
        
        builder.Property(x => x.Gender)
            .HasConversion<string>()
            .HasColumnName("Genero");
        
        builder.Property(x => x.Condition)
            .HasConversion<string>()
            .HasColumnName("Condicion");

        builder.HasOne(x=> x.Church)
            .WithMany(x=>x.Campers)
            .HasForeignKey(x=>x.ChurchId);
        
        builder.HasOne(x=>x.Room)
            .WithMany(x=>x.Campers)
            .HasForeignKey(x=>x.RoomId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}