using System.Text;
using Microsoft.Extensions.Options;
using PolAssessment.AnprEnricher.Configuration;

namespace PolAssessment.AnprEnricher.Services;

public interface IVehicleDataUrlService
{
    string GetVehicleDataUrl(string licensePlate);
}

public class VehicleDataUrlService(IOptions<VehicleEnricherConfig> config) : IVehicleDataUrlService
{
    private readonly VehicleEnricherConfig _config = config.Value;

    public string GetVehicleDataUrl(string licensePlate)
    {
        var stringBuilder = new StringBuilder(_config.BaseUrl);
        stringBuilder.AppendFormat(_config.Query, licensePlate);

        return stringBuilder.ToString();
    }
}
