using Microsoft.Extensions.Options;
using PolAssessment.AnprEnricher.Configuration;

namespace PolAssessment.AnprEnricher.Services;

interface IDataSendThreadControlService
{
    Task WaitAsync();
    int Release();
}

public class DataSendThreadControlService(IOptions<AnprDataProcessorConfig> config) : SemaphoreSlim(config.Value.ConcurrentSendData), IDataSendThreadControlService
{

}