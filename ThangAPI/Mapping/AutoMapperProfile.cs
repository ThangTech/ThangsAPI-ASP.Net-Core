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
            CreateMap<AddWalkDTO, Walkcs>().ReverseMap();
            CreateMap<Walkcs, WalkDTO>().ReverseMap();
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
            CreateMap<UpdateWalkDTO, Walkcs>().ReverseMap();
        }
    }
}
