namespace SMJRegisterAPI.Features.Camper.Dtos;

public class CreateCamperDTO
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public int PaidAmount { get; set; }
    public bool IsGrant { get; set; }
    public int Gender { get; set; } 
    public int Condition { get; set; }
    
    //relationship
    public int ChurchId { get; set; }
}