using Bogus;
using Microsoft.EntityFrameworkCore;
using SMJRegisterAPI.Entities;

namespace SMJRegisterAPI.Database.Seeds;

public class RoomSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var faker = new Faker<Room>("es")
            .RuleFor(p=>p.Capacity , f=>f.Random.Int(2,51));
        
    }
}