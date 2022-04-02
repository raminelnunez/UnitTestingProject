namespace AdvancedCSharpFinalProject.Models
{
    public class Developer : ApplicationUser //inherits from ApplicationUser
    {
        public ICollection<ProjectTask> ProjectTasks { get; set; }
        public ICollection<Note> Notes { get; set; }
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
            Notes = new HashSet<Note>();
        }
        public Developer()
        {

        }
    }
}
