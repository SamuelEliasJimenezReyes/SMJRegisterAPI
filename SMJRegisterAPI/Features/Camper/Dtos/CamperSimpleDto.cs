using SMJRegisterAPI.Features.Church.Dtos;

namespace SMJRegisterAPI.Features.Camper.Dtos;

public class CamperSimpleDto
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string DocumentNumber { get; set; }
    public int  PaidAmount { get; set; }
    public bool IsGrant { get; set; } 
    public string Gender { get; set; }
    public string Condition { get; set; }
    
    public ChurchSimpleDTO Church { get; set; }
}