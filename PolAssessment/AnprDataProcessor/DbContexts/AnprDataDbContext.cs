using Microsoft.EntityFrameworkCore;
using PolAssessment.Shared.Models;

namespace PolAssessment.AnprDataProcessor.DbContexts;

public interface IAnprDataDbContext
{
    DbSet<AnprRecord> AnprRecords { get; set; }
    DbSet<UploadUser> UploadUsers { get; set; }
    DbSet<AnprRecordUploadUser> AnprRecordUploadUsers { get; set; }
}

public class AnprDataDbContext(DbContextOptions<AnprDataDbContext> options) : DbContext(options), IAnprDataDbContext
{
    public DbSet<AnprRecord> AnprRecords { get; set; }
    public DbSet<UploadUser> UploadUsers { get; set; }
    public DbSet<AnprRecordUploadUser> AnprRecordUploadUsers { get; set; }
}
