using SMJRegisterAPI.Common.Entities;
using SMJRegisterAPI.Entities.Enums;

namespace SMJRegisterAPI.Entities;

public class Camper : BaseEntity
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public int  PaidAmount { get; set; }
    
    public bool IsGrant { get; set; } = false;
    
    public Gender Gender { get; set; }
    
    public Condition Condition { get; set; }

    public int ChurchId { get; set; }
    public Church Church { get; set; }
}