namespace PolAssessment.AnprEnricher.App.Models;

public class AnprData
{
    public required string Plate { get; set; }
    public required Coordinates Coordinates { get; set; }
    public DateTime DateTime { get; set; }
}
