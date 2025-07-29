using SMJRegisterAPI.Features.Church.Dtos;
using SMJRegisterAPI.Features.GrantedCode.Dtos;
using SMJRegisterAPI.Features.Room.Dtos;
namespace SMJRegisterAPI.Features.Camper.Dtos;

public class CamperDTO
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string DocumentNumber { get; set; }
    public int  PaidAmount { get; set; }
    public bool IsGrant { get; set; } 
    public string Gender { get; set; }
    public string Condition { get; set; }
    public List<string>? DocumentsURL { get; set; }

    
    public ChurchSimpleDTO Church { get; set; }
    public GrantedCodeSimpleDTO? GrantedCode { get; set; }
    public RoomSimpleDto? Room { get; set; }
}