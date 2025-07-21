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
        if (camper.IsGrant)
        {
            var camperDb = await repository.GetByIdAsync(camper.ID);
    
            var grantedCode = await grantedCodeRepository.AddWithCodeAsync(
                new Entities.GrantedCode()
                {
                    CamperId = camper.ID,
                },
                request.Camper.GrantedAmount);

            camper.GrantedCodeId = grantedCode.ID;
            grantedCode.Camper = camper;
    
            await repository.LoadReferenceAsync(camper, c => c.GrantedCode);
        }
        await repository.LoadReferenceAsync(camper,c=>c.Church);
        await repository.LoadReferenceAsync(camper,c=>c.Room);

        var Dto = mapper.Map<CreateCamperDTO>(camper);
        return Dto;
    }
}