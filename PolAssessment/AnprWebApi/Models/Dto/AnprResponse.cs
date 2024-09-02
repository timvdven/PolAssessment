using PolAssessment.Shared.Models;

namespace PolAssessment.AnprWebApi.Models.Dto;

public class AnprResponse : BaseApiResponse
{
    public required string Hash { get; set; }
    public required IEnumerable<AnprRecord> Result { get; set; }
}
