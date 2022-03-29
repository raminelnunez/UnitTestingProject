using AdvancedCSharpFinalProject.Data;
using AdvancedCSharpFinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdvancedCSharpFinalProject.Controllers
{
    public class MainController : Controller
    {
        private ApplicationDbContext _db { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }
        private RoleManager<IdentityRole> _roleManager { get; set; }

        public MainController(ApplicationDbContext Db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = Db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult GetAllRolesForAUser(string? userId)
        {
            ViewBag.usersList = new SelectList(_db.Users.ToList(), "Id", "UserName");
            UserManager userManager = new UserManager(_db, _userManager, _roleManager);
            List<string> roleNamesOfUser = userManager.GetAllRolesOfUser(userId);// GetAllRolesOfUser method on UserManager Class
            if (userManager.User != null)
            {
                ViewBag.UserName = userManager.User.UserName;
            }
            return View(roleNamesOfUser);
        }
        [Authorize(Roles = "Project Manager")]
        public IActionResult AssignRoleToUser()
        {
            ViewBag.usersList = new SelectList(_db.Users.ToList(), "Id", "UserName");
            ViewBag.rolesList = new SelectList(_db.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(string userId, string roleForUser)
        {
            UserManager userManager = new UserManager(_db, _userManager, _roleManager);
            string message = await userManager.AssignRoleToUser(userId, roleForUser); // AssignRoleToUser method on UserManager Class
            ViewBag.message = message;
            return View("MessageView");
        }
    }
}

/*
Create a User Manager class that has functions to manage users and roles 
(Get all roles for a user, assign roles to users, check if a user in a role)...etc
 */