using AdvancedCSharpFinalProject.Data;
using AdvancedCSharpFinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public IActionResult ViewProject(int ProjectId, bool? OrderByCompletion, bool? OrderByPriority, bool? HideComplete)
        {
            Project Project = _db.Project.First(p => p.Id == ProjectId);
            List<ProjectTask>? ProjectTasks = _db.ProjectTask.Include(t => t.Developer).ThenInclude(d => d.UserName).Where(t => t.ProjectId == ProjectId).ToList();

            if (OrderByCompletion != null)
            {
                if ((bool)OrderByCompletion)
                {
                    ProjectTasks = ProjectTasks.OrderBy(t => t.CompletionPercentage).ToList();
                }
            }

            if (OrderByPriority != null)
            {
                if ((bool)OrderByPriority)
                {
                    ProjectTasks = ProjectTasks.OrderBy(t => t.Priority).ToList();
                }
            }

            if (HideComplete != null)
            {
                if ((bool)HideComplete)
                {
                    ProjectTasks = ProjectTasks.Where(t => t.IsCompleted == false).ToList();
                }
            }

            Project.ProjectTasks = ProjectTasks;
            return View(Project);
        }

        public IActionResult CreateTask(int ProjectId)
        {
            // work in progress
            return View(ProjectId);
        }
        public IActionResult ViewAllProjects()
        {
            List<Project> Projects = _db.Project.ToList();
            return View(Projects);
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
        public async Task<IActionResult> SelectDailySalary(string? userId, string? roleForUser)
        {
            if(userId != null && roleForUser != null)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                if(await _userManager.IsInRoleAsync(user, roleForUser))
                {
                    ViewBag.message = $"{user.UserName} is already in the role of {roleForUser}";
                    return View("MessageView");
                }
                else
                {
                    ViewBag.userId = userId;
                    ViewBag.roleForUser = roleForUser;
                    return View();
                }
            } 
            else
            {
                return BadRequest("Bad Request");
            }

        }
        [HttpPost]
        public async Task<IActionResult> SelectDailySalary(string? userId, string? roleForUser, double dailySalary)
        {
            if (userId != null && roleForUser != null)
            {
                try
                {
                    UserManager userManager = new UserManager(_db, _userManager, _roleManager);
                    string message = await userManager.AssignRoleToUser(userId, roleForUser, dailySalary); // AssignRoleToUser method on UserManager Class
                    ViewBag.message = message;
                    return View("MessageView");
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("Bad Request");
            }
        }
        public IActionResult CheckIfAUserIsInARole()
        {
            ViewBag.usersList = new SelectList(_db.Users.ToList(), "Id", "UserName");
            ViewBag.rolesList = new SelectList(_db.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CheckIfAUserIsInARole(string? userId, string? roleToCheck)
        {
            if(userId != null && roleToCheck != null)
            {
                try
                {
                    UserManager userManager = new UserManager(_db, _userManager, _roleManager);
                    string message = await userManager.CheckIfAUserIsInARole(userId, roleToCheck);
                    ViewBag.message = message;
                    return View("MessageView");
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("Bad Request");
            }

        }
        [Authorize(Roles = "Project Manager")]
        public IActionResult AddANewProject()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddANewProject([Bind("Title, AssignedBudget, Deadline, Priority")] Project newProject)//Bind properties that the form will provide you
        {
            //clear validation for properties you need but will not get from the form
            ModelState.ClearValidationState("ProjectManager");
            ModelState.ClearValidationState("ProjectManagerId");
            ModelState.ClearValidationState("ActualBudget");//we need to add ourselves
            ModelState.ClearValidationState("IsCompleted");//we need to add ourselves
            ModelState.ClearValidationState("CompletionPercentage");// we need to add ourselves
            ModelState.ClearValidationState("ProjectTasks");//ProjectTasks needs to be instantiated


            //add them manually
            //Properties we need in order to create Project
            ApplicationUser projectManager = await _userManager.FindByNameAsync(User.Identity.Name);

            ProjectHelper projectHelper = new ProjectHelper(_userManager, projectManager);
            newProject = projectHelper.AddProject(newProject);

            if (TryValidateModel(newProject))
            {
                await _userManager.UpdateAsync(projectManager);
                return View("Index");
            }
            return View();
        }
        public IActionResult UpdateProject()
        {
            return View();
        }
    }
}

/*
Create a User Manager class that has functions to manage users and roles 
(Get all roles for a user, assign roles to users, check if a user in a role)...etc

Create a ProjectHelper class that contains functions to add, delete, update projects, 
along with any other helper functions. 
Those functions can only be accessed by project managers.

A Project manager will have a dashboard page which shows all the projects with their related tasks 
and assigned developers, the projects with high priorities appear first.
 */