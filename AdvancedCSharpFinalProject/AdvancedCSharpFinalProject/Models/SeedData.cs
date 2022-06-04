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

                //user1 as a Developer
                ApplicationUser firstUserDeveloper = new ApplicationUser()
                {
                    Email = "musab@gmail.com",
                    NormalizedEmail = "MUSAB@GMAIL.COM",
                    UserName = "musab@gmail.com",
                    NormalizedUserName = "MUSAB@GMAIL.COM",
                    EmailConfirmed = true,
                    DailySalary = 100,
                    IsDeveloper = true,
                };
                var firstUserDeveloperHashedPassword = passwordHasher.HashPassword(firstUserDeveloper, "Pass@12");
                firstUserDeveloper.PasswordHash = firstUserDeveloperHashedPassword;
                await userManager.CreateAsync(firstUserDeveloper);
                await userManager.AddToRoleAsync(firstUserDeveloper, "Developer");


                //user2 as a ProjectManager and a Developer
                ApplicationUser secondUserProjectManager = new ApplicationUser() //Discriminator will say ProjectManager(because we instantiate it as a ProjectManager)
                {
                    Email = "raminel@gmail.com",
                    NormalizedEmail = "RAMINEL@GMAIL.COM",
                    UserName = "raminel@gmail.com",
                    NormalizedUserName = "RAMINEL@GMAIL.COM",
                    EmailConfirmed = true,
                    DailySalary = 100,//Developer.DailySalary
                    IsProjectManager = true,
                    IsDeveloper = true,

                };
                var secondUserProjectManagerHashedPassword = passwordHasher.HashPassword(secondUserProjectManager, "Pass@12");
                secondUserProjectManager.PasswordHash = secondUserProjectManagerHashedPassword;

                await userManager.CreateAsync(secondUserProjectManager);
                await userManager.AddToRoleAsync(secondUserProjectManager, "Project Manager");
                await userManager.AddToRoleAsync(secondUserProjectManager, "Developer");

                //A User can be a ProjectManager and a Developer

                //testUser is just a user(potential to become a Developer and ProjectManager)
                ApplicationUser testUserDeveloper = new ApplicationUser()
                {
                    Email = "testuser01@gmail.com",
                    NormalizedEmail = "TESTUSER01@GMAIL.COM",
                    UserName = "testuser01@gmail.com",
                    NormalizedUserName = "TESTUSER01@GMAIL.COM",
                    EmailConfirmed = true,
                };
                var testUserDeveloperHashedPassword = passwordHasher.HashPassword(testUserDeveloper, "Pass@12");
                testUserDeveloper.PasswordHash = testUserDeveloperHashedPassword;
                await userManager.CreateAsync(testUserDeveloper);
            }
        }
    }
}
