using SMJRegisterAPI.Common.Entities;

namespace SMJRegisterAPI.Entities;

public class Room : BaseEntity
{
    public string Name { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentCapacity { get; set; }
    
    //relationships
    public ICollection<Camper> Campers { get; set; }
}