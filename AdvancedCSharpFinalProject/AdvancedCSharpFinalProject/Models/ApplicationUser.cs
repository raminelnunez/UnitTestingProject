using Microsoft.AspNetCore.Identity;

namespace AdvancedCSharpFinalProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public double DailySalary { get; set; }
    }
}
