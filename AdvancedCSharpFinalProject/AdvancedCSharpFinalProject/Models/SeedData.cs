using AdvancedCSharpFinalProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCSharpFinalProject.Models
{
    public class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!context.Roles.Any())
            {
                //create new roles

                List<string> newRoles = new List<string>()
                {
                    "Project Manager",
                    "Developer",
                };
                foreach (string role in newRoles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            if (!context.Users.Any())
            {
                //create new users
                var passwordHasher = new PasswordHasher<ApplicationUser>();

                //user1
                ApplicationUser firstUser = new ApplicationUser()
                {
                    Email = "musab03@gmail.com",
                    NormalizedEmail = "MUSAB03@GMAIL.COM",
                    UserName = "musab03@gmail.com",
                    NormalizedUserName = "MUSAB03@GMAIL.COM",
                    EmailConfirmed = true,
                };
                var firstUserHashedPassword = passwordHasher.HashPassword(firstUser, "Pass@12");
                firstUser.PasswordHash = firstUserHashedPassword;
                await userManager.CreateAsync(firstUser);
                await userManager.AddToRoleAsync(firstUser, "Developer");


                //user2
                ApplicationUser secondUser = new ApplicationUser()
                {
                    Email = "raminel03@gmail.com",
                    NormalizedEmail = "RAMINEL03@GMAIL.COM",
                    UserName = "raminel03@gmail.com",
                    NormalizedUserName = "RAMINEL03@GMAIL.COM",
                    EmailConfirmed = true,
                };
                var secondUserHashedPassword = passwordHasher.HashPassword(secondUser, "Pass@12");
                secondUser.PasswordHash = secondUserHashedPassword;

                await userManager.CreateAsync(secondUser);
                await userManager.AddToRoleAsync(secondUser, "Project Manager");
            }
        }
    }
}
