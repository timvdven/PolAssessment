namespace PolAssessment.AnprDataProcessor.Models.ViewModels;

public class AuthenticateRequest
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}