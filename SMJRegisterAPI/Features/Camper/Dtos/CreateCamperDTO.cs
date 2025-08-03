namespace SMJRegisterAPI.Features.Camper.Dtos;

public class CreateCamperDTO
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string DocumentNumber { get; set; }
    public int PaidAmount { get; set; }
    public bool IsGrant { get; set; }
    public decimal GrantedAmount { get; set; }
    public bool IsPaid { get; set; } = false;
    public IReadOnlyList<IFormFile>? Documents { get; set; }
    public int Gender { get; set; } 
    public int Condition { get; set; }
    public int PayType { get; set; }
    public int ShirtSize { get; set; }
    public DateTime ArrivedTime { get; set; }

    //relationship
    public int ChurchId { get; set; }
    public int? RoomId { get; set; }
    public string? Code { get; set; }
}