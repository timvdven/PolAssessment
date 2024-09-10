using System.Net;

namespace PolAssessment.Common.Lib.Models;

public abstract class BaseWebResponse
{
    public bool Success { get; set; }
    public HttpStatusCode HttpResponseCode { get; set; }
    public string? ErrorMessage { get; set; }
}