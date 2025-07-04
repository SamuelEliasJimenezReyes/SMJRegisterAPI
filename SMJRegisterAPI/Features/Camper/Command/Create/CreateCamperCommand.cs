using MediatR;
using SMJRegisterAPI.Features.Camper.Dtos;

namespace SMJRegisterAPI.Features.Camper.Command.Create;

public class CreateCamperCommand : IRequest<CreateCamperDTO>
{
    public CreateCamperCommand(CreateCamperDTO camper)
    {
        Camper = camper;
    }

    public CreateCamperDTO Camper { get; set; }
    
}