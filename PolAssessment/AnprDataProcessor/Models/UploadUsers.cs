using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PolAssessment.AnprDataProcessor.Models;

public class UploadUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public required string UserDescription { get; set; }

    [Required]
    [StringLength(64)]
    public required string ClientId { get; set; }

    [Required]
    [StringLength(64)]
    public required string ClientSecret { get; set; }
}
