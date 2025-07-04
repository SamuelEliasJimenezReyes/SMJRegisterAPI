using AutoMapper;
using MediatR;
using SMJRegisterAPI.Features.Camper.Dtos;
using SMJRegisterAPI.Features.Camper.Repository;

namespace SMJRegisterAPI.Features.Camper.Queries.GetById;

public class GetCamperByIdQueryHandler(ICamperRepository repository, IMapper mapper)
    : IRequestHandler<GetCamperByIdQuery, CamperDTO>
{

    public async Task<CamperDTO> Handle(GetCamperByIdQuery request, CancellationToken cancellationToken)
    {
        var entityDb = await repository.GetByIdAsync(request.ID);
        return mapper.Map<CamperDTO>(entityDb);
    }
}