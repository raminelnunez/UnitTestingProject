using AdvancedCSharpFinalProject.Data;
using Microsoft.AspNetCore.Identity;

namespace AdvancedCSharpFinalProject.Models
{
    public class UserManager // we don't need this Class in our database
    {
        public ApplicationDbContext _db { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }
        public RoleManager<IdentityRole> _roleManager { get; set; }
        public ApplicationUser User { get; set; }
        public List<string> GetAllRolesOfUser(string userId)
        {
            User = _db.Users.FirstOrDefault(user => user.Id == userId); 
            List<string> allRoleIdsOfUser = _db.UserRoles.Where(userRole => userRole.UserId == userId).Select(userRole => userRole.RoleId).ToList();
            List<string> roleNames = _db.Roles.Where(roles => allRoleIdsOfUser.Contains(roles.Id)).Select(roles => roles.Name).ToList();
            return roleNames;
        }
        public async Task<string> AssignRoleToUser(string userId, string roleForUser) //does not return anything
        {
            try
            {
                string message;
                ApplicationUser user = await _userManager.FindByIdAsync(userId); // pull the user out of the database to assign the role to it 
                User = user;

                if (await _roleManager.RoleExistsAsync(roleForUser)) // check if role is in the database
                {
                    if (!await _userManager.IsInRoleAsync(user, roleForUser)) // check if the user is already in that role
                    {
                        await _userManager.AddToRoleAsync(user, roleForUser);//if user doesn't have the role add it to the user
                        //await _userManager.RemoveFromRoleAsync(user, roleForUser);//if I were to remove a role from a user I would use this
                        message = $"{User.UserName} is added to role {roleForUser}";
                        return message;
                    }
                    else
                    {
                        message = $"{User.UserName} is already in role {roleForUser}";
                        return message;
                    }
                }else
                {
                    message = $"No role found";
                    return message;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return message;
            }

        }
        public UserManager()
        {

        }
        public UserManager(ApplicationDbContext Db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = Db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
    }
}
