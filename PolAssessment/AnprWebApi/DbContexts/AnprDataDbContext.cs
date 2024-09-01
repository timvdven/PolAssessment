using Microsoft.EntityFrameworkCore;
using PolAssessment.Shared.Models;

namespace PolAssessment.AnprWebApi.DbContexts;

public interface IAnprDataDbContext
{
    DbSet<AnprRecord> AnprRecords { get; set; }
}

public class AnprDataDbContext(DbContextOptions<AnprDataDbContext> options) : DbContext(options), IAnprDataDbContext
{
    public DbSet<AnprRecord> AnprRecords { get; set; }
}
