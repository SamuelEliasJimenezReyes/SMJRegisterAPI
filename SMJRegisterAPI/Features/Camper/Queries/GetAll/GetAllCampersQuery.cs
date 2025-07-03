using MediatR;
using SMJRegisterAPI.Features.Camper.Dtos;

namespace SMJRegisterAPI.Features.Camper.Queries.GetAll;

public class GetAllCampersQuery : IRequest<IList<CamperDTO>>
{
    
}