using SMJRegisterAPI.Features.Common;

namespace SMJRegisterAPI.Features.Room.Repository;

public interface IRoomRepository : IGenericRepository<Entities.Room>
{
    Task<IEnumerable<Entities.Room>> GetAllRoomsWhitCamper();
    Task<Entities.Room> GetByIdWithCamper(int id);
}