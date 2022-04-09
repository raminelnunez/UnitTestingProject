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
    [Authorize]
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
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserManager userManager = new UserManager(_db, _userManager, _roleManager);
            List<string> roleNamesOfCurrentUser = userManager.GetAllRolesOfUser(user.Id);// GetAllRolesOfUser method on UserManager Class
            if (roleNamesOfCurrentUser.Any())
            {
                ViewBag.NoRolesForCurrentUser = false;
            }
            else
            {
                ViewBag.NoRolesForCurrentUser = true;
            }
            return View();
        }
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
            if (userId != null && roleForUser != null)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                if (await _userManager.IsInRoleAsync(user, roleForUser))
                {
                    ViewBag.message = $"{user.UserName} is already in the role of {roleForUser}";
                    ViewBag.action = "Index";
                    ViewBag.actionMessage = "Back to Index";
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
                    ViewBag.action = "Index";
                    ViewBag.actionMessage = "Back to Index";
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
            if (userId != null && roleToCheck != null)
            {
                try
                {
                    UserManager userManager = new UserManager(_db, _userManager, _roleManager);
                    string message = await userManager.CheckIfAUserIsInARole(userId, roleToCheck);
                    ViewBag.message = message;
                    ViewBag.action = "Index";
                    ViewBag.actionMessage = "Back to Index";
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
        public async Task<IActionResult> ViewProject(int ProjectId, string? orderType)
        {
            Project Project = _db.Project.Include(project => project.ProjectManager).First(p => p.Id == ProjectId);
            List<ProjectTask>? ProjectTasks = _db.ProjectTask.Include(t => t.Developer).Where(t => t.ProjectId == ProjectId).ToList();
            ApplicationUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.currentUser = currentUser;
            List<SelectListItem> orderBySelectList = new List<SelectListItem>()
            {
                new SelectListItem("Order By Completion", "OrderByCompletion"),
                new SelectListItem("Order By Priority", "OrderByPriority"),
                new SelectListItem("Hide Completed Task", "HideComplete"),
            };
            ViewBag.orderBySelectList = orderBySelectList;
            if (orderType != null)
            {
                if (orderType == "OrderByCompletion")
                {
                    ProjectTasks = ProjectTasks.OrderBy(t => t.CompletionPercentage).ToList();
                }
            }

            if (orderType != null)
            {
                if (orderType == "OrderByPriority")
                {
                    ProjectTasks = ProjectTasks.OrderByDescending(t => t.Priority).ToList();
                }
            }

            if (orderType != null)
            {
                if (orderType == "HideComplete")
                {
                    ProjectTasks = ProjectTasks.Where(t => t.IsCompleted == false).ToList();
                }
            }

            Project.ProjectTasks = ProjectTasks;
            return View(Project);
        }

        [Authorize(Roles = "Project Manager")]
        public IActionResult CreateTask(int ProjectId)
        {
            Project project =  _db.Project.First(project => project.Id == ProjectId);
            ViewBag.ProjectTitle = project.Title;
            ViewBag.ProjectId = ProjectId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Project Manager")]
        public IActionResult CreateTask([Bind("ProjectId, Priority, Title, Description, Deadline")] ProjectTask newProjectTask)
        {
            //Project project = _db.Project.First(p => p.Id == projectId);
            //ProjectTask task = new ProjectTask();
            //task = new ProjectTask(project, title, description, deadline, priority);
            //TaskHelper taskHelper = new TaskHelper();
            //taskHelper.AddTask(_db, task);

            //return Redirect($"ViewProject?ProjectId={project.Id}");

            ModelState.ClearValidationState("IsCompleted");//we need to add ourselves
            ModelState.ClearValidationState("CompletionPercentage");// we need to add ourselves
            ModelState.ClearValidationState("Project");// we need to add ourselves

            newProjectTask.IsCompleted = false;
            newProjectTask.CompletionPercentage = 0;

            Project projectOfTheTask = _db.Project
                .Include(project => project.ProjectTasks)
                .Include(project => project.ProjectManager)
                .First(project => project.Id == newProjectTask.ProjectId);
            projectOfTheTask.ProjectTasks.Add(newProjectTask);
            newProjectTask.Project = projectOfTheTask;

            TaskHelper taskHelper = new TaskHelper();
            taskHelper.AddTask(_db, newProjectTask);

            if (TryValidateModel(newProjectTask))
            {
                _db.SaveChanges();
                return RedirectToAction("ViewProject", new { ProjectId = projectOfTheTask.Id });
            }
            return View();

        }
        public IActionResult ViewTask(int? taskId)
        {
            if(taskId != null)
            {
                try
                {
                    ProjectTask taskToView = _db.ProjectTask
                        .Include(projectTask => projectTask.Developer)
                        .Include(projectTask => projectTask.Project).ThenInclude(project => project.ProjectManager)
                        .First(projectTask => projectTask.Id == taskId);
                    return View(taskToView);
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("taskId is null");
            }

        }
        [Authorize(Roles = "Project Manager")]
        public IActionResult AssignTaskToDeveloper(int? taskId)
        {
            if(taskId != null)
            {
                try
                {
                    List<ApplicationUser> developers = _db.Users.Where(user => user.IsDeveloper == true).ToList();
                    ViewBag.developerList = new SelectList(developers, "Id", "UserName");
                    ProjectTask taskToAssign = _db.ProjectTask
                       .Include(projectTask => projectTask.Developer)
                       .Include(projectTask => projectTask.Project).ThenInclude(project => project.ProjectManager)
                       .First(projectTask => projectTask.Id == taskId);
                    return View(taskToAssign);

                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("taskId is null");
            }
        }
        [Authorize(Roles = "Project Manager")]
        [HttpPost]
        public IActionResult AssignTaskToDeveloper(int? taskId, string? developerId)
        {
            if(taskId !=null && developerId != null)
            {
                try
                {
                    ApplicationUser developer = _db.Users
                        .First(user => user.Id == developerId);

                    ProjectTask taskToAssign = _db.ProjectTask
                        .Include(task => task.Developer)
                        .First(task => task.Id == taskId);

                    TaskHelper taskHelper = new TaskHelper();
                    taskHelper.AddDeveloperToTask(_db, taskToAssign, developer);

                    _db.SaveChanges();
                    return RedirectToAction("ViewProject", new { ProjectId = taskToAssign.ProjectId });
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("taskId or developerId is null");
            }
        }
        public IActionResult UpdateTask(int? taskId)
        {
            if(taskId != null)
            {
                try
                {
                    ProjectTask taskToUpdate = _db.ProjectTask
                        .Include(task => task.Developer)
                        .Include(task => task.Project)
                        .First(task => task.Id == taskId);
                    return View(taskToUpdate);
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("taskId is null");
            }
        }
        [HttpPost]
        public IActionResult UpdateTask(int? taskId, ProjectTask updatedTask)
        {
            if(taskId != null)
            {
                try
                {
                    ProjectTask task = _db.ProjectTask
                       .Include(task => task.Developer)
                       .Include(task => task.Project)
                       .First(task => task.Id == taskId);
                    TaskHelper taskHelper = new TaskHelper();

                    taskHelper.UpdateTask(task, updatedTask);
                    _db.SaveChanges();

                    return RedirectToAction("ViewTask", new {taskId = task.Id});
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("taskId is null at Post");
            }
        }
        public IActionResult DeleteWarningForTask(int? taskId)
        {
            if(taskId != null)
            {
                try
                {
                    ProjectTask task = _db.ProjectTask
                      .First(task => task.Id == taskId);

                    ViewBag.message = $"Task: <b style=\"color:purple\">{task.Title}</b> will be deleted permanently";
                    ViewBag.abortAction = "ViewProject";
                    ViewBag.taskId = taskId;
                    ViewBag.projectId = task.ProjectId;
                    return View();
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("taskId is null at DeleteWarningForTask");
            }
        }
        [Authorize(Roles = "Project Manager")]
        public IActionResult DeleteTask(int? taskId)
        {
            if(taskId != null)
            {
                try
                {
                    ProjectTask taskToDelete = _db.ProjectTask
                       .Include(task => task.Developer)
                       .Include(task => task.Project)
                       .First(task => task.Id == taskId);
                    TaskHelper taskHelper = new TaskHelper();
                    taskHelper.DeleteTask(_db, taskToDelete);
                    _db.SaveChanges();
                    return RedirectToAction("ViewProject", new { ProjectId = taskToDelete.ProjectId });

                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("taskId is null at DeleteTask");
            }
        }
        public IActionResult ViewAllProjects()
        {
            List<Project> Projects = _db.Project
                .Include(project => project.ProjectManager)
                .OrderByDescending(project => project.Priority)
                .ToList();
            return View(Projects);
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

            ProjectHelper projectHelper = new ProjectHelper(projectManager);
            newProject = projectHelper.AddProject(newProject);

            if (TryValidateModel(newProject))
            {
                await _userManager.UpdateAsync(projectManager);
                ViewBag.message = $"Project: <b style=\"color:purple\">{newProject.Title}</b> created";
                ViewBag.action = "ViewAllProjects";
                ViewBag.actionMessage = "Back to Projects";
                return View("MessageView");
            }
            return View();
        }
        [Authorize(Roles = "Project Manager")]
        public IActionResult UpdateProject(int? projectId)
        {
            if(projectId != null)
            {
                try
                {
                    Project projectToUpdate = _db.Project.First(project => project.Id == projectId);
                    return View(projectToUpdate);
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("projectId is null");
            }
        }
        [Authorize(Roles = "Project Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProject(int? projectId, Project updatedProject)
        {
            if(projectId != null)
            {
                try
                {
                    Project project = _db.Project.First(project => project.Id == projectId);
                    ProjectHelper projectHelper = new ProjectHelper();
                    projectHelper.UpdateProject(project, updatedProject);
                    _db.SaveChanges();
                    ViewBag.message = $"Project: <b style=\"color:purple\">{project.Title}</b> has been updated";
                    ViewBag.action = "ViewAllProjects";
                    ViewBag.actionMessage = "Back to Projects";
                    return View("MessageView");
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("projectId is null");
            }
        }
        [Authorize(Roles = "Project Manager")]
        public IActionResult DeleteWarning(int? projectId)
        {
            Project project = _db.Project.First(project => project.Id == projectId);
            ViewBag.message = $"Project: <b style=\"color:purple\">{project.Title}</b> will be deleted permanently";
            ViewBag.abortAction = "ViewAllProjects";
            ViewBag.ProjectId = projectId;
            return View();
        }
        [Authorize(Roles = "Project Manager")]
        public IActionResult DeleteProject(int? projectId)
        {
            if(projectId != null)
            {
                try
                {
                    Project projectToDelete = _db.Project.First(project => project.Id == projectId);
                    ProjectHelper projectHelper = new ProjectHelper(_db);
                    projectHelper.DeleteProject(projectToDelete);
                    _db.SaveChanges();
                    ViewBag.message = $"Project: <b style=\"color:purple\">{projectToDelete.Title}</b> has been deleted";
                    ViewBag.action = "ViewAllProjects";
                    ViewBag.actionMessage = "Back to Projects";
                    return View("MessageView");
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("projectId is null");
            }
        }
    }
}

/*
Create a User Manager class that has functions to manage users and roles 
(Get all roles for a user, assign roles to users, check if a user in a role)...etc

Create a ProjectHelper class that contains functions to add, delete, update projects, 
along with any other helper functions. 
Those functions can only be accessed by project managers.

Create a TaskHelper class that contains functions to add, delete, update tasks, and assign a task to a developer, 
those functions can only be accessed by project managers.

A Project manager will have a dashboard page which shows all the projects with their related tasks 
and assigned developers, the projects with high priorities appear first.

Developers can view all their tasks (but can’t view other developers’ tasks), 
also a developer can update a task to change the completed percentage of the task, or mark it totally completed, 
when the task is marked completed, the developer can leave a comment to describe any notes or hints`.
 */