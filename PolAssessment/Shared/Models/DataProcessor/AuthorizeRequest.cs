namespace PolAssessment.Shared.Models.DataProcessor
{
    public class AuthorizeRequest
    {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
    }
}
