using SMJRegisterAPI.Features.Camper.Dtos;

namespace SMJRegisterAPI.Features.Room.Dtos;

public class RoomDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    
    public ICollection<CamperDTO> Campers { get; set; }
}