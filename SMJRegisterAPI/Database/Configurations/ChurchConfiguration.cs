using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMJRegisterAPI.Entities;
using SMJRegisterAPI.Entities.Enums;

namespace SMJRegisterAPI.Database.Configurations;


public class ChurchConfiguration(Conference tenantConference) : IEntityTypeConfiguration<Church>
{
    

    public void Configure(EntityTypeBuilder<Church> builder)
    {
        builder.ToTable("Iglesias");
        
        builder.HasKey(x => x.ID);
        
        builder.HasQueryFilter(x => !x.IsDeleted && (
            tenantConference== Conference.General || x.Conference == tenantConference));
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .HasColumnName("Nombre");
        
        builder.Property(x => x.Conference)
            .HasConversion<string>()
            .HasColumnName("Conferencia");
    }
}