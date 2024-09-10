using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PolAssessment.Common.Lib.Models;

namespace PolAssessment.AnprDataProcessor.WebApi.DbContexts;

public interface IAnprDataDbContext
{
    DbSet<AnprRecord> AnprRecords { get; set; }
    DbSet<UploadUser> UploadUsers { get; set; }
    DbSet<AnprRecordUploadUser> AnprRecordUploadUsers { get; set; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class AnprDataDbContext(DbContextOptions<AnprDataDbContext> options) : DbContext(options), IAnprDataDbContext
{
    public DbSet<AnprRecord> AnprRecords { get; set; }
    public DbSet<UploadUser> UploadUsers { get; set; }
    public DbSet<AnprRecordUploadUser> AnprRecordUploadUsers { get; set; }
}
