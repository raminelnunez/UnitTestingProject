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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //set primary key
            builder.Entity<Project>().HasKey(project => project.Id);
            builder.Entity<ProjectTask>().HasKey(projectTask => projectTask.Id);

            //ProjectManager to Project (One To Many)
            builder.Entity<ProjectManager>()
                .HasMany(projectManager => projectManager.Projects)
                .WithOne(project => project.ProjectManager)
                .HasForeignKey(project => project.ProjectManagerId);

            //Project to Developer (Many To Many)
            //breakTable: ProjectTask

            //for Developer to ProjectTask
            builder.Entity<ProjectTask>()
                .HasOne(projectTask => projectTask.Developer)
                .WithMany(developer => developer.ProjectTasks)
                .HasForeignKey(projectTask => projectTask.DeveloperId);

            //for Project to ProjectTask
            builder.Entity<ProjectTask>()
                .HasOne(projectTask => projectTask.Project)
                .WithMany(project => project.ProjectTasks)
                .HasForeignKey(projectTask => projectTask.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            //ProjectTask to Developer (Many to Many)
            //breakTable: Note

        }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectTask> ProjectTask { get; set; }
    }
}