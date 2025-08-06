using ABC.Models.Domain;
using ABC.Models.DTO;
using AutoMapper;

namespace ABC.Mappings
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles() 
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap();
        }
    }
}
