using SMJRegisterAPI.Common.Entities;
using SMJRegisterAPI.Entities.Enums;

namespace SMJRegisterAPI.Entities;

public class Camper : BaseEntity
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string DocumentNumber { get; set; }
    public int  PaidAmount { get; set; }
    public bool IsGrant { get; set; } = false;
    public bool IsPaid { get; set; } = false;
    public Gender Gender { get; set; }
    public Condition Condition { get; set; }
    
    //RelationsShip
    public int ChurchId { get; set; }
    public Church Church { get; set; }
    
    public int? RoomId { get; set; }
    public Room Room { get; set; }
    
    public int? GrantedCodeId { get; set; }
    public GrantedCode GrantedCode { get; set; }
}