using AutoMapper;
using ThangAPI.Models.Domain;
using ThangAPI.Models.DTO;

namespace ThangAPI.Mapping
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionDTO, Region>().ReverseMap();
            CreateMap<UpdateRegionDTO, Region>().ReverseMap();
        }
    }
}
