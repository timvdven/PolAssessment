using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PolAssessment.Common.Lib.Models;

public class AnprRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [StringLength(20)]
    public required string LicensePlate { get; set; }

    [Required]
    public double Longitude { get; set; }

    [Required]
    public double Latitude { get; set; }

    [Required]
    public DateTime ExactDateTime { get; set; }

    [StringLength(255)]
    public string? VehicleTechnicalName { get; set; }

    [StringLength(255)]
    public string? VehicleBrandName { get; set; }

    public DateTime? VehicleApkExpirationDate { get; set; }

    [StringLength(255)]
    public string? LocationStreet { get; set; }

    [StringLength(255)]
    public string? LocationCity { get; set; }
}
