using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMJRegisterAPI.Entities;

namespace SMJRegisterAPI.Database.Configurations;


public class ChurchConfiguration : IEntityTypeConfiguration<Church>
{
    public void Configure(EntityTypeBuilder<Church> builder)
    {
        builder.ToTable("Iglesias");
        
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .HasColumnName("Nombre");
        
        builder.Property(x => x.Conference)
            .HasConversion<string>()
            .HasColumnName("Conferencia");
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .HasColumnName("PrimerNombre");
    }
}