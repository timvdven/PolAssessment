using PolAssessment.Common.Lib.Models;

namespace PolAssessment.AnprWebApi.Models.Dto;

public class AnprResponse : BaseWebResponse
{
    public required string Hash { get; set; }
    public required DateTime LastFetchDate { get; set; }
    public required IEnumerable<AnprRecord> Result { get; set; }
}
