using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PolAssessment.AnprWebApi.Models;

public class WebUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [Required]
    [StringLength(128)]
    public required string Fullname { get; set; }

    [Required]
    [StringLength(64)]
    public required string Username { get; set; }

    [Required]
    [StringLength(64)]
    public required string HashedPassword { get; set; }
}
