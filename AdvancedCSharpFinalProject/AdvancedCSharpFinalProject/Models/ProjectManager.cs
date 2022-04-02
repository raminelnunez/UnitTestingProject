namespace AdvancedCSharpFinalProject.Models
{
    public class ProjectManager : ApplicationUser
    {
        public ProjectManager(ApplicationUser user)
        {
            Email = user.Email;
            NormalizedEmail = user.NormalizedEmail;
            UserName = user.UserName;
            NormalizedUserName = user.NormalizedUserName;
            EmailConfirmed = true;
            PasswordHash = user.PasswordHash;
            DailySalary = user.DailySalary;
        }
        public ProjectManager()
        {

        }
    }
}
