using AutoMapper;
using MediatR;
using SMJRegisterAPI.Features.Camper.Dtos;
using SMJRegisterAPI.Features.Camper.Repository;
using SMJRegisterAPI.Features.GrantedCode.Repository;

namespace SMJRegisterAPI.Features.Camper.Command.Create;

public class CreateCamperCommandHandler(ICamperRepository repository,IGrantedCodeRepository grantedCodeRepository, IMapper mapper)
    : IRequestHandler<CreateCamperCommand, CreateCamperDTO>
{

    public async Task<CreateCamperDTO> Handle(CreateCamperCommand request, CancellationToken cancellationToken)
    {
        var camper = mapper.Map<Entities.Camper>(request.Camper);
        
        camper.Gender = (Entities.Enums.Gender)request.Camper.Gender;
        camper.Condition = (Entities.Enums.Condition)request.Camper.Condition;
        await repository.AddAsync(camper);

        if (camper.IsGrant && !String.IsNullOrWhiteSpace(request.Camper.Code))
        {
            var grantedCode = await grantedCodeRepository.GetByCodeAsync(request.Camper.Code);
            grantedCode.IsUsed = true;
            grantedCode.CamperId = camper.ID;
            camper.GrantedCodeId = grantedCode.ID;

            await grantedCodeRepository.UpdateAsync(grantedCode, grantedCode.ID);
            camper.UpdatedAt = DateTime.Now;
            await repository.UpdateAsync(camper, camper.ID);
        }
        await repository.LoadReferenceAsync(camper,c=>c.Church);
        await repository.LoadReferenceAsync(camper,c=>c.Room);

        var Dto = mapper.Map<CreateCamperDTO>(camper);
        return Dto;
    }
}