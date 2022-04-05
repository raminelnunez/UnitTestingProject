using Microsoft.AspNetCore.Identity;

namespace AdvancedCSharpFinalProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public double DailySalary { get; set; }
        public ICollection<ProjectTask> ProjectTasks { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ApplicationUser()
        {
            Projects = new HashSet<Project>();
            ProjectTasks = new HashSet<ProjectTask>();
        }

    }
}
