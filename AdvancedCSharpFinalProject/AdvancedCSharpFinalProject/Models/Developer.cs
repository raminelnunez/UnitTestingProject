namespace AdvancedCSharpFinalProject.Models
{
    public class Developer : ApplicationUser //inherits from ApplicationUser
    {
        public Developer(ApplicationUser user)
        {
            Id = user.Id;
            Email = user.Email;
            NormalizedEmail = user.NormalizedEmail;
            UserName = user.UserName;
            NormalizedUserName = user.NormalizedUserName;
            EmailConfirmed = true;
            PasswordHash = user.PasswordHash;
            DailySalary = user.DailySalary;
            IsProjectManager = user.IsProjectManager;
            IsDeveloper = user.IsDeveloper;

            ProjectTasks = new HashSet<ProjectTask>();
        }
        public Developer()
        {

        }
    }
}
