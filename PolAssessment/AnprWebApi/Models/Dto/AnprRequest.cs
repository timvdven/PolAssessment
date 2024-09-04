namespace PolAssessment.AnprWebApi.Models.Dto
{
    public class AnprRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Plate { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? Hash { get; set; }
        public DateTime? MinimumUploadDate { get; set; }
    }
}
