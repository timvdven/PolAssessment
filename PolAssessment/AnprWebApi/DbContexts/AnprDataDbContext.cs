using Microsoft.EntityFrameworkCore;
using PolAssessment.Shared.Models;

namespace PolAssessment.AnprWebApi.DbContexts;

public interface IAnprDataDbContext
{
    DbSet<AnprRecord> AnprRecords { get; set; }
    DbSet<AnprRecordUploadUser> AnprRecordUploadUsers { get; set; }
}

public class AnprDataDbContext(DbContextOptions<AnprDataDbContext> options) : DbContext(options), IAnprDataDbContext
{
    public DbSet<AnprRecord> AnprRecords { get; set; }
    public DbSet<AnprRecordUploadUser> AnprRecordUploadUsers { get; set; }
}
