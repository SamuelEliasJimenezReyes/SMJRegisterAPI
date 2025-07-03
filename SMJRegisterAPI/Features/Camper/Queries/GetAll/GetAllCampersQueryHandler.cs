using AutoMapper;
using MediatR;
using SMJRegisterAPI.Database.Contexts;
using SMJRegisterAPI.Features.Camper.Dtos;
using SMJRegisterAPI.Features.Camper.Repository;
using SMJRegisterAPI.Features.Common;

namespace SMJRegisterAPI.Features.Camper.Queries.GetAll;

public class GetAllCampersQueryHandler(ICamperRepository repository, IMapper mapper)
    : IRequestHandler<GetAllCampersQuery, IList<CamperDTO>>
{
    

    public async Task<IList<CamperDTO>> Handle(GetAllCampersQuery request, CancellationToken cancellationToken)
    {
        var list = await repository.GetAllAsync();
        return mapper.Map<IList<CamperDTO>>(list);
    }
}