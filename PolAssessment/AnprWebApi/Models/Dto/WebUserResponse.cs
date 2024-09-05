using PolAssessment.Shared.Models;

namespace PolAssessment.AnprWebApi.Models.Dto;

public class WebUserResponse : BaseWebResponse
{
    public int Id { get; set; }
    public required string Fullname { get; set; }
    public required string Username { get; set; }
}
