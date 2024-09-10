using Microsoft.EntityFrameworkCore;
using PolAssessment.AnprWebApi.Models;

namespace PolAssessment.AnprWebApi.DbContexts;

public interface IWebApiDbContext
{
    DbSet<WebUser> WebUsers { get; set; }
}

public class WebApiDbContext(DbContextOptions<WebApiDbContext> options) : DbContext(options), IWebApiDbContext
{
    public DbSet<WebUser> WebUsers { get; set; }
}
