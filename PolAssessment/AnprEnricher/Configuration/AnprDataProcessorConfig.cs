namespace PolAssessment.AnprEnricher.Configuration;

public class AnprDataProcessorConfig
{
    public required Operations Operation { get; set; }
    public required string BaseUrl { internal get; set; }
    public required string ClientId { internal get; set; }
    public required string ClientSecret { internal get; set; }
    public int MaxRetries { internal get; set; }
    public int RetryDelay { internal get; set; }

    public class Operations
    {
        public required string Authorize { get; set; }
        public required string Anpr { get; set; }
    }
}
