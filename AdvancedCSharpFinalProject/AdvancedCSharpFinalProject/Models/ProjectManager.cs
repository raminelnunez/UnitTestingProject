namespace AdvancedCSharpFinalProject.Models
{
    public class ProjectManager : ApplicationUser
    {
        public ICollection<Project> Projects { get; set;}
        public ProjectManager(ApplicationUser user)
        {
            Email = user.Email;
            NormalizedEmail = user.NormalizedEmail;
            UserName = user.UserName;
            NormalizedUserName = user.NormalizedUserName;
            EmailConfirmed = true;
            PasswordHash = user.PasswordHash;
            DailySalary = user.DailySalary;

            Projects = new HashSet<Project>();
        }
        public ProjectManager()
        {

        }
    }
}
