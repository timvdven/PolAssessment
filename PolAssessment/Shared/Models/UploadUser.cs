using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PolAssessment.Shared.Models;

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
    public required string HashedClientSecret { get; set; }
}
