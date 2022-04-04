namespace AdvancedCSharpFinalProject.Models
{
    public class ProjectManager : ApplicationUser
    {
        public ProjectManager(ApplicationUser user)
        {
            Id = user.Id;
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
