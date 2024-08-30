using Microsoft.EntityFrameworkCore;
using PolAssessment.Shared.Models;

namespace PolAssessment.AnprWebApi.DbContexts;

public class AnprDbContext(DbContextOptions<AnprDbContext> options) : DbContext(options)
{
    public DbSet<AnprRecord> AnprRecords { get; set; }
}
