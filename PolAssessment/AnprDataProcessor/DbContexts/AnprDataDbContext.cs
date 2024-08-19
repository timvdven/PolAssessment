using Microsoft.EntityFrameworkCore;
using PolAssessment.AnprDataProcessor.Models;

namespace PolAssessment.AnprDataProcessor.DbContexts;

public class AnprDataDbContext(DbContextOptions<AnprDataDbContext> options) : DbContext(options)
{
    public DbSet<AnprRecord> AnprRecords { get; set; }
    public DbSet<UploadUser> UploadUsers { get; set; }
}
