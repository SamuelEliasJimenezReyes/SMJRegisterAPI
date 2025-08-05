using Bogus;
using Microsoft.EntityFrameworkCore;
using SMJRegisterAPI.Entities;
using SMJRegisterAPI.Entities.Enums;

namespace SMJRegisterAPI.Database.Seeds;

public class BankInformationSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var id = 1;
        var faker = new Faker<BanksInformation>("es")
            .RuleFor(p => p.ID, f => id++)
            .RuleFor(x => x.Conference, f => f.PickRandom<Conference>())
            .RuleFor(x => x.BankName, f => f.PickRandom<Banks>())
            .RuleFor(x => x.Cedula, f => f.Random.String2(11, "0123456789"))
            .RuleFor(x => x.AccountNumber, f =>f.Random.String2(11, "0123456789"));
        
        foreach (var entity in faker.Generate(20))
            modelBuilder.Entity<BanksInformation>().HasData(entity); 
    }

}