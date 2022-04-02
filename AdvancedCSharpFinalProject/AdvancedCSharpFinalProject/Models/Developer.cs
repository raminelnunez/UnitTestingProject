namespace AdvancedCSharpFinalProject.Models
{
    public class Developer : ApplicationUser //inherits from ApplicationUser
    {
        public ICollection<ProjectTask> ProjectTasks { get; set; }
        public Developer(ApplicationUser user)
        {
            Email = user.Email;
            NormalizedEmail = user.NormalizedEmail;
            UserName = user.UserName;
            NormalizedUserName = user.NormalizedUserName;
            EmailConfirmed = true;
            PasswordHash = user.PasswordHash;
            DailySalary = user.DailySalary;

            ProjectTasks = new HashSet<ProjectTask>();
        }
        public Developer()
        {

        }
    }
}
