using Bogus;
using Microsoft.EntityFrameworkCore;
using SMJRegisterAPI.Entities;
using SMJRegisterAPI.Entities.Enums;

namespace SMJRegisterAPI.Database.Seeds;

public class ChurchSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var id = 1;
        var faker = new Faker<Church>("es")
            .RuleFor(p => p.ID, f => id++)
            .RuleFor(x=>x.Name,f=>f.Address.City())
            .RuleFor(x=>x.Conference,f=>f.PickRandom<Conference>());

        foreach (var entity in faker.Generate(10))
            modelBuilder.Entity<Church>().HasData(entity);
    }
}
