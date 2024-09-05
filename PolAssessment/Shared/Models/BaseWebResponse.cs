using System.Net;

namespace PolAssessment.Shared.Models
{
    public abstract class BaseWebResponse
    {
        public bool Success { get; set; }
        public HttpStatusCode HttpResponseCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}