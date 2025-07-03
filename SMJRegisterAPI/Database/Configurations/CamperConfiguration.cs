using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMJRegisterAPI.Entities;

namespace SMJRegisterAPI.Database.Configurations;

public class CamperConfiguration : IEntityTypeConfiguration<Camper>
{
    public void Configure(EntityTypeBuilder<Camper> builder)
    {
        builder.ToTable("Campers");
        
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.HasKey(x => x.ID);
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .HasColumnName("Nombre");
        
        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .HasColumnName("Apellido");
        
        builder.Property(x => x.PaidAmount)
            .HasColumnName("CantidadPaga");
        
        builder.Property(x => x.IsGrant)
            .HasColumnName("Becado");
        
        builder.Property(x => x.Gender)
            .HasConversion<string>()
            .HasColumnName("Genero");
        
        builder.HasOne(x=> x.Church)
            .WithMany(x=>x.Campers)
            .HasForeignKey(x=>x.ChurchId);
    }
}