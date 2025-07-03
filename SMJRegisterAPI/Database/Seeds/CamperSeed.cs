using Bogus;
using Microsoft.EntityFrameworkCore;
using SMJRegisterAPI.Database.Contexts;
using SMJRegisterAPI.Entities;
using SMJRegisterAPI.Entities.Enums;

namespace SMJRegisterAPI.Database.Seeds;

public class CamperSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var id = 1;
        var faker = new Faker<Camper>("es")
            .RuleFor(p => p.ID, f => id++)
            .RuleFor(x => x.Name, f => f.Person.FirstName)
            .RuleFor(x => x.LastName, f => f.Person.LastName)
            .RuleFor(x => x.Gender, f => f.PickRandom<Gender>())
            .RuleFor(x => x.Condition, f => f.PickRandom<Condition>())
            .RuleFor(x => x.PaidAmount, f => f.Random.Number(0, 2500))
            .RuleFor(x => x.ChurchId, f => f.Random.Number(1, 10));
        
        foreach (var entity in faker.Generate(50))
            modelBuilder.Entity<Camper>().HasData(entity);
    }
}