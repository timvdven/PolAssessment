using System.Net;

namespace PolAssessment.AnprWebApi.Models.Dto;

public abstract class BaseApiResponse
{
    public HttpStatusCode HttpStatusCode { get; set; }
    public string? Message { get; set; }
    public bool Success { get; set; }
}
