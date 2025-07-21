using SMJRegisterAPI.Features.Camper.Dtos;

namespace SMJRegisterAPI.Features.GrantedCode.Dtos;

public class GrantedCodeDTO
{
    public string Code { get; set; }
    public int GrantAmount { get; set; }
    public bool IsUsed { get; set; } = false;
    
}