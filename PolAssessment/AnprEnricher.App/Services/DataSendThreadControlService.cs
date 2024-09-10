using Microsoft.Extensions.Options;
using PolAssessment.AnprEnricher.App.Configuration;

namespace PolAssessment.AnprEnricher.App.Services;

public interface IDataSendThreadControlService
{
    Task WaitAsync();
    int Release();
}

public class DataSendThreadControlService(IOptions<AnprDataProcessorConfig> config) : SemaphoreSlim(config.Value.ConcurrentSendData), IDataSendThreadControlService
{

}