using AutoMapper;
using Infrastructure.Models.Dto;
using QuickFixAPI.Models;

namespace QuickFixAPI.Mappings
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {

            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Fix, FixDto>();
                config.CreateMap<FixDto, Fix>();
            });
            return mappingConfig;
        }
    }
}
