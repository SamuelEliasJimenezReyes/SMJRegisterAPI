using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMJRegisterAPI.Entities;
using SMJRegisterAPI.Entities.Enums;

namespace SMJRegisterAPI.Database.Configurations;

public class BanksInformationConfiguration(Conference tenantConference) : IEntityTypeConfiguration<BanksInformation>
{
    public void Configure(EntityTypeBuilder<BanksInformation> builder)
    {
        builder.ToTable("Cuentas");
        
        builder.HasKey(x => x.ID);

        builder.HasQueryFilter(x => !x.IsDeleted && (
            tenantConference== Conference.General || x.Conference == tenantConference));
        
        builder.Property(x => x.Cedula)
            .HasMaxLength(100);
        
        builder.Property(x => x.AccountNumber)
            .HasMaxLength(100)
            .HasColumnName("NumeroDeCuenta");
        
        builder.Property(x => x.Conference)
            .HasConversion<string>()
            .HasColumnName("Conferencia");
        
        builder.Property(x => x.BankName)
            .HasConversion<string>()
            .HasColumnName("Banco");
    }
}