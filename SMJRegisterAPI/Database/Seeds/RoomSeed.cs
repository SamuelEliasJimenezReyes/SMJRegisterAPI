using Bogus;
using Microsoft.EntityFrameworkCore;
using SMJRegisterAPI.Entities;

namespace SMJRegisterAPI.Database.Seeds;

public class RoomSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var roomsNames = new List<string>()
        {
            "Habitación alta",
            "Habitación baja",
            "Atras del Comedor",
            "Dormitorio Izquierda",
            "Dormitorio Derecha",
            "Piso 3"
        };

        
        var faker = new Faker<Room>("es")
            .RuleFor(p=>p.Capacity , f=>f.Random.Int(2,51))
            .RuleFor(p=>p.Name, f=>f.PickRandom(roomsNames));
        
        foreach (var entity in faker.Generate(5))
            modelBuilder.Entity<Camper>().HasData(entity);
        
    }
}