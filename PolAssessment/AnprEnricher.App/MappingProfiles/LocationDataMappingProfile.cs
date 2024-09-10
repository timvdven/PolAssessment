using AutoMapper;
using PolAssessment.AnprEnricher.App.Models;
using PolAssessment.AnprEnricher.App.Models.Dto;

namespace PolAssessment.AnprEnricher.App.MappingProfiles;

public class LocationDataMappingProfile : Profile
{
    public LocationDataMappingProfile()
    {
        CreateMap<LocationDataDto.Properties, EnrichedLocationData>();
    }
}
