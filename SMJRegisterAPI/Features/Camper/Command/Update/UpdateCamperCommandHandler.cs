using AutoMapper;
using MediatR;
using SMJRegisterAPI.Features.Camper.Dtos;
using SMJRegisterAPI.Features.Camper.Repository;
using SMJRegisterAPI.Features.GrantedCode.Repository;
using SMJRegisterAPI.Services.FileStore;

namespace SMJRegisterAPI.Features.Camper.Command.Update;

public class UpdateCamperCommandHandler(
    ICamperRepository repository,
    IGrantedCodeRepository grantedCodeRepository,
    IMapper mapper,
    IFileStorage storage)
    : IRequestHandler<UpdateCamperCommand, UpdateCamperDTO>
{
    public async Task<UpdateCamperDTO> Handle(UpdateCamperCommand request, CancellationToken cancellationToken)
    {
        var camper = await repository.GetByIdAsync(request.Id);

        // Solo se actualizan los campos permitidos
        camper.Name = request.Camper.Name;
        camper.LastName = request.Camper.LastName;
        camper.PaidAmount = request.Camper.PaidAmount;
        camper.IsGrant = request.Camper.IsGrant;
        camper.IsPaid = request.Camper.IsPaid;
        camper.Gender = (Entities.Enums.Gender)request.Camper.Gender;
        camper.Condition = (Entities.Enums.Condition)request.Camper.Condition;
        camper.PayType = (Entities.Enums.PayType)request.Camper.PayType;
        camper.ShirtSize = (Entities.Enums.ShirtSize)request.Camper.ShirtSize;
        camper.ChurchId = request.Camper.ChurchId;
        camper.RoomId = request.Camper.RoomId;

        camper.UpdatedAt = DateTime.Now;

        await repository.UpdateAsync(camper, camper.ID);

        var camperDto = mapper.Map<UpdateCamperDTO>(camper);
        return camperDto;
    }
}
