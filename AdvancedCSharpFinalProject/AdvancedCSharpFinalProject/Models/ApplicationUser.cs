using Microsoft.AspNetCore.Identity;

namespace AdvancedCSharpFinalProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public double DailySalary { get; set; }
        public ICollection<ProjectTask> ProjectTasks { get; set; }
        public ICollection<Project> Projects { get; set; }
        public bool IsDeveloper { get; set; }
        public bool IsProjectManager { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ApplicationUser()
        {
            Projects = new HashSet<Project>();
            ProjectTasks = new HashSet<ProjectTask>();
            Comments = new HashSet<Comment>();
            Notifications = new HashSet<Notification>();
        }

    }
}
