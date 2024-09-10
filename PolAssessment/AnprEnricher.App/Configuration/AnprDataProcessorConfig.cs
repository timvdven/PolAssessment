namespace PolAssessment.AnprEnricher.App.Configuration;

public class AnprDataProcessorConfig
{
    public required OperationConfig Operation { get; set; }
    public required string BaseUrl { get; set; }
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public int MaxRetries { get; set; }
    public int RetryDelay { get; set; }
    public int ConcurrentSendData { get; set; }

    public class OperationConfig
    {
        public required string Authorize { get; set; }
        public required string Anpr { get; set; }
    }
}
