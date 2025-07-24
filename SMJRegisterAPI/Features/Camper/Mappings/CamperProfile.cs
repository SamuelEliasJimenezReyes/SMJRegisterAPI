using AutoMapper;
using SMJRegisterAPI.Features.Camper.Command.Create;
using SMJRegisterAPI.Features.Camper.Dtos;

namespace SMJRegisterAPI.Features.Camper.Mappings;

public class CamperProfile : Profile
{
    public CamperProfile()
    {
        CreateMap<Entities.Camper, CamperDTO>()
            .ForMember(dest=>dest.Church, opt
                =>opt.MapFrom(
                    src=>src.Church))
            .ForMember(dest=>dest.GrantedCode, opt=>opt.MapFrom(
                src=>src.GrantedCode))
            .ForMember(dest=>dest.Room,opt=>opt.MapFrom(
                src=>src.Room));
        CreateMap<Entities.Camper, CreateCamperDTO>();
        
        CreateMap<Entities.Camper, CamperSimpleDto>()
            .ForMember(dest=>dest.Church,
                opt=>opt.MapFrom(
                src=>src.Church));

        CreateMap<CreateCamperDTO, Entities.Camper>();
        CreateMap<CreateCamperCommand , Entities.Camper>();
    }
    
}