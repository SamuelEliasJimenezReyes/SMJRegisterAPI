using AutoMapper;
using MediatR;
using SMJRegisterAPI.Features.Camper.Dtos;
using SMJRegisterAPI.Features.Camper.Repository;
using SMJRegisterAPI.Features.GrantedCode.Repository;
using SMJRegisterAPI.Services.FileStore;
namespace SMJRegisterAPI.Features.Camper.Command.Create;

public class CreateCamperCommandHandler(ICamperRepository repository,
    IGrantedCodeRepository grantedCodeRepository, 
    IMapper mapper,
    IFileStorage storage)
    : IRequestHandler<CreateCamperCommand, CreateCamperDTO>
{

    public async Task<CreateCamperDTO> Handle(CreateCamperCommand request, CancellationToken cancellationToken)
    {
        var camper = mapper.Map<Entities.Camper>(request.Camper);
        
        camper.Gender = (Entities.Enums.Gender)request.Camper.Gender;
        camper.Condition = (Entities.Enums.Condition)request.Camper.Condition;
        camper.PayType = (Entities.Enums.PayType)request.Camper.PayType;
        camper.ShirtSize = (Entities.Enums.ShirtSize)request.Camper.ShirtSize;
        if(request.Camper.RoomId == 0)
                camper.RoomId = null;   

        var startDate = new DateTime(2025, 11, 1);
        var endDate = new DateTime(2025, 11, 5);
        var pricePerDay = 100m; 
        
        var totalDays=  (endDate - request.Camper.ArrivedTime).Days + 1;
        camper.TotalAmount = totalDays * pricePerDay;

        if (camper.IsGrant && request.Camper.GrantedAmount>0)
        {
            camper.TotalAmount = request.Camper.GrantedAmount;
            if (camper.TotalAmount < 0)
                camper.TotalAmount = 0;
        }
        await repository.AddAsync(camper);

        if (request.Camper.Documents.Any())
        {
            var folderName = $"Camper-{camper.ID}-{camper.Name}-{camper.LastName}";
            var urls = await storage.MultipleStore("camper-documents", folderName, request.Camper.Documents);
            camper.DocumentsURL = urls;
            camper.UpdatedAt = DateTime.Now;
            await repository.UpdateAsync(camper,camper.ID);
        }
        
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
        var Dto = mapper.Map<CreateCamperDTO>(camper);
        return Dto;
    }
}