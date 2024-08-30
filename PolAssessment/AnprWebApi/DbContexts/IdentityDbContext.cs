using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PolAssessment.AnprWebApi.DbContexts;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : IdentityDbContext<IdentityUser>(options)
{

}
