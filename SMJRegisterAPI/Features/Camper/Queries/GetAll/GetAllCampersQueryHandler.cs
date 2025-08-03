using System.Globalization;
using AutoMapper;
using Humanizer;
using MediatR;
using SMJRegisterAPI.Database.Contexts;
using SMJRegisterAPI.Features.Camper.Dtos;
using SMJRegisterAPI.Features.Camper.Repository;
using SMJRegisterAPI.Features.Common;

namespace SMJRegisterAPI.Features.Camper.Queries.GetAll;

public class GetAllCampersQueryHandler(ICamperRepository repository, IMapper mapper)
    : IRequestHandler<GetAllCampersQuery, IList<CamperSimpleDto>>
{
    
    public async Task<IList<CamperSimpleDto>> Handle(GetAllCampersQuery request, CancellationToken cancellationToken)
    {
        var list = await repository.GetAllAsync();
        var mapped =  mapper.Map<IList<CamperSimpleDto>>(list);
        CultureInfo.CurrentCulture = new CultureInfo("es");
        foreach (var camper in mapped)
        {
            camper.ArrivedTimeHumanized = camper.ArrivedTime.Humanize(true,null,new CultureInfo("es"));
        }
        return mapped;
    }
}