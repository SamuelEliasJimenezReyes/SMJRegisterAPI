using MediatR;
using SMJRegisterAPI.Features.Camper.Repository;
using SMJRegisterAPI.Features.Room.Repository;

namespace SMJRegisterAPI.Features.Room.Command.AutomaticSorterByChurch;

public class AutomaticSorterByChurchCommandHandler(IRoomRepository repository, ICamperRepository camperRepository) 
    : IRequestHandler<AutomaticSorterByChurchCommand, Unit>
{
    public async Task<Unit> Handle(AutomaticSorterByChurchCommand request, CancellationToken cancellationToken)
    {
        var campers = await camperRepository.GetAllByChurchIDAsync(request.ChurchId);
        var unassinged = campers.Where(x => x.RoomId is null or 0 ).ToList();

        var rooms = await repository.GetAllRoomsWhitCamper();

        foreach (var camper in unassinged)
        {
            var availableRoom = rooms.FirstOrDefault(r => r.Campers.Count < r.Capacity);

            if (availableRoom is not null)
            {
                camper.RoomId = availableRoom.ID;
                availableRoom.Campers.Add(camper);
                await camperRepository.UpdateAsync(camper, camper.ID);
            }
        }
        return Unit.Value;
    }
}