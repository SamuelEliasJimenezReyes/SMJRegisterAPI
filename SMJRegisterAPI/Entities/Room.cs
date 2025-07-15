using SMJRegisterAPI.Common.Entities;

namespace SMJRegisterAPI.Entities;

public class Room : BaseEntity
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    
    //relationships
    public ICollection<Camper> Campers { get; set; }
}