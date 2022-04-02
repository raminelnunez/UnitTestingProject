using AdvancedCSharpFinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCSharpFinalProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProjectManager> ProjectManager { get; set; }
        public DbSet<Developer> Developer { get; set; }
        public DbSet<Project> Project { get; set; }
    }
}