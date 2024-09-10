using AutoMapper;
using PolAssessment.AnprEnricher.App.Models;
using PolAssessment.Common.Lib.Models;

namespace PolAssessment.AnprEnricher.App.MappingProfiles;

public class AnprRecordProfile : Profile
{
    public AnprRecordProfile()
    {
        CreateMap<AnprData, AnprRecord>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.Plate))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Coordinates.Longitude))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Coordinates.Latitude))
            .ForMember(dest => dest.ExactDateTime, opt => opt.MapFrom(src => src.DateTime));

        CreateMap<EnrichedVehicleData, AnprRecord>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.VehicleTechnicalName, opt => opt.MapFrom(src => src.TechnicalName))
            .ForMember(dest => dest.VehicleBrandName, opt => opt.MapFrom(src => src.BrandName))
            .ForMember(dest => dest.VehicleApkExpirationDate, opt => opt.MapFrom(src => src.ApkExpirationDate));

        CreateMap<EnrichedLocationData, AnprRecord>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.LocationStreet, opt => opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.LocationCity, opt => opt.MapFrom(src => src.City));
    }
}
