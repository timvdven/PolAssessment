using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PolAssessment.Common.Lib.Models;

[PrimaryKey(nameof(AnprRecordId), nameof(UploadUserId))]
public class AnprRecordUploadUser
{
    public long AnprRecordId { get; set; }

    public int UploadUserId { get; set; }

    [Required]
    public DateTime UploadDateTime { get; set; } = DateTime.UtcNow;

    [ForeignKey("AnprRecordId")]
    public virtual AnprRecord? AnprRecord { get; set; }

    [ForeignKey("UploadUserId")]
    public virtual UploadUser? UploadUser { get; set; }
}
