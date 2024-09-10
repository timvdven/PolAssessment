using AutoMapper;
using PolAssessment.AnprEnricher.App.Models;
using PolAssessment.AnprEnricher.App.Models.Dto;

namespace PolAssessment.AnprEnricher.App.MappingProfiles;

public class VehicleDataMappingProfile : Profile
{
    public VehicleDataMappingProfile()
    {
        CreateMap<VehicleDataDto.Properties, EnrichedVehicleData>()
            .ForMember(dest => dest.ApkExpirationDate, opt => opt.MapFrom(src => ParseDate(src.ApkExpirationDate)));
    }

    private static DateTime ParseDate(string? candidate)
    {
        if (candidate == null)
        {
            return DateTime.MinValue;
        }

        return DateTime.ParseExact(candidate, "yyyyMMdd", null);
    }
}
