using AutoMapper;
using SMJRegisterAPI.Features.Camper.Dtos;

namespace SMJRegisterAPI.Features.Camper.Mappings;

public class CamperProfile : Profile
{
    public CamperProfile()
    {
        CreateMap<Entities.Camper, CamperDTO>()
            .ForMember(dest=>dest.Church, opt
                =>opt.MapFrom(
                    src=>src.Church));
    }
    
}