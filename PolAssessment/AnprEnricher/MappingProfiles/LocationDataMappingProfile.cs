using AutoMapper;
using PolAssessment.AnprEnricher.Models;
using PolAssessment.AnprEnricher.Models.Dto;

namespace PolAssessment.AnprEnricher.MappingProfiles;

public class LocationDataMappingProfile : Profile
{
    public LocationDataMappingProfile()
    {
        CreateMap<LocationDataDto.Properties, EnrichedLocationData>();
    }
}
