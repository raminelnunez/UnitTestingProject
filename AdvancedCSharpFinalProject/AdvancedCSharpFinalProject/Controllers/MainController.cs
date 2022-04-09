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
            try
            {
                DateTime currentDate = DateTime.Now.Date;
                ApplicationUser currentUser = _db.Users
                    .Include(user => user.Projects)
                    .Include(user => user.Notifications)
                    .Include(user => user.ProjectTasks)
                    .ThenInclude(task => task.Project)
                    .First(user => user.UserName == User.Identity.Name);
                UserManager userManager = new UserManager(_db, _userManager, _roleManager);
                List<string> roleNamesOfCurrentUser = userManager.GetAllRolesOfUser(currentUser.Id);// GetAllRolesOfUser method on UserManager Class
                if (roleNamesOfCurrentUser.Any())
                {
                    ViewBag.NoRolesForCurrentUser = false;
                }
                else
                {
                    ViewBag.NoRolesForCurrentUser = true;
                }
                if(currentUser.IsDeveloper == true)
                {
                    if(currentUser.ProjectTasks.Any())
                    {
                        foreach(ProjectTask task in currentUser.ProjectTasks)
                        {
                            //(EndDate.Date - StartDate.Date).Days
                            double daysLeft = (task.Deadline.Date - currentDate).TotalDays;
                            if (daysLeft <= 1 && task.IsNotified == false)
                            {
                                Notification notification = new Notification(currentUser, $"{task.Title}: {daysLeft} day left till deadline {task.Deadline}");
                                currentUser.Notifications.Add(notification);
                                task.IsNotified = true;
                                _db.Notification.Add(notification);
                                _db.SaveChanges();
                            }
                        }
                    }

                }
                return View(currentUser);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message + "At Index");
            }
            //A developer will get a notification when their task is only one day to pass the deadline.
            //The Developers should be able to see the number of notifications in their homepage,
            //they can click on this number to go to a page where they can see all the notifications in detail.
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
            Project project = _db.Project.First(project => project.Id == ProjectId);
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
            ModelState.ClearValidationState("Comments");// we need to add ourselves
            try
            {
                newProjectTask.IsCompleted = false;
                newProjectTask.CompletionPercentage = 0;
                newProjectTask.Comments = new HashSet<Comment>();

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
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }



        }
        public async Task<IActionResult> ViewTask(int? taskId)
        {
            if (taskId != null)
            {
                try
                {
                    ApplicationUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    ViewBag.currentUser = currentUser;
                    ProjectTask taskToView = _db.ProjectTask
                        .Include(projectTask => projectTask.Developer)
                        .Include(projectTask => projectTask.Comments)
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
            if (taskId != null)
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
        [HttpPost]
        public IActionResult AssignTaskToDeveloper(int? taskId, string? developerId)
        {
            if (taskId != null && developerId != null)
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
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("taskId or developerId is null");
            }
        }
        [Authorize(Roles = "Developer, ProjectManager")]
        public IActionResult UpdateTask(int? taskId)
        {
            if (taskId != null)
            {
                try
                {
                    ProjectTask taskToUpdate = _db.ProjectTask
                        .Include(task => task.Developer)
                        .Include(task => task.Project)
                        .First(task => task.Id == taskId);
                    return View(taskToUpdate);
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
        [HttpPost]
        [Authorize(Roles = "Developer, ProjectManager")]
        public IActionResult UpdateTask(int? taskId, ProjectTask updatedTask)
        {
            if (taskId != null)
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

                    return RedirectToAction("ViewTask", new { taskId = task.Id });
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("taskId is null at Post");
            }
        }
        [Authorize(Roles = "Project Manager")]
        public IActionResult DeleteWarningForTask(int? taskId)
        {
            if (taskId != null)
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
            if (taskId != null)
            {
                try
                {
                    ProjectTask taskToDelete = _db.ProjectTask
                       .Include(task => task.Developer)
                       .Include(task => task.Project)
                       .Include(task => task.Comments)
                       .First(task => task.Id == taskId);
                    _db.Comment.RemoveRange(taskToDelete.Comments.ToList());
                    TaskHelper taskHelper = new TaskHelper();
                    taskHelper.DeleteTask(_db, taskToDelete);
                    _db.SaveChanges();
                    return RedirectToAction("ViewProject", new { ProjectId = taskToDelete.ProjectId });

                }
                catch (Exception ex)
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
            if (projectId != null)
            {
                try
                {
                    Project projectToUpdate = _db.Project.First(project => project.Id == projectId);
                    return View(projectToUpdate);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProject(int? projectId, Project updatedProject)
        {
            if (projectId != null)
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
            if (projectId != null)
            {
                try
                {
                    Project projectToDelete = _db.Project
                        .Include(project => project.ProjectTasks)
                        .ThenInclude(task => task.Comments)
                        .First(project => project.Id == projectId);

                    List<Comment> commentsToDelete = _db.Comment
                        .Include(comment => comment.ProjectTask)
                        .Where(comment => comment.ProjectTask.ProjectId == projectId).ToList();

                    _db.Comment.RemoveRange(commentsToDelete);
                    _db.ProjectTask.RemoveRange(projectToDelete.ProjectTasks.ToList());

                    ProjectHelper projectHelper = new ProjectHelper(_db);
                    projectHelper.DeleteProject(projectToDelete);
                    _db.SaveChanges();
                    ViewBag.message = $"Project: <b style=\"color:purple\">{projectToDelete.Title}</b> has been deleted";
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
        [Authorize(Roles = "Developer")]
        public IActionResult AddCommentToTask(int? taskId, string? developerId)
        {
            ViewBag.taskId = taskId;
            ViewBag.developerId = developerId;
            return View();
        }
        [Authorize(Roles = "Developer")]
        [HttpPost]
        public async Task<IActionResult> AddCommentToTask([Bind("Content, CommentDate, ProjectTaskId, DeveloperId")] Comment newComment)
        {
            ModelState.ClearValidationState("Developer");
            ModelState.ClearValidationState("ProjectTask");

            ApplicationUser? developer = await _userManager.FindByNameAsync(User.Identity.Name);


            ProjectTask projectTask = _db.ProjectTask // NEEDS everything from all objects
                .Include(projectTask => projectTask.Developer)
                .Include(projectTask => projectTask.Project)
                .ThenInclude(project => project.ProjectManager)
                .Include(projectTask => projectTask.Comments)
                .First(task => task.Id == newComment.ProjectTaskId);

            newComment.Developer = developer;
            newComment.DeveloperId = developer.Id;
            newComment.ProjectTask = projectTask;

            developer.Comments.Add(newComment);
            projectTask.Comments.Add(newComment);
            _db.Comment.Add(newComment);

            if (TryValidateModel(newComment))
            {
                _db.SaveChanges();
                return RedirectToAction("ViewTask", new { taskId = projectTask.Id });
            }
            return View();
        }
        [Authorize(Roles = "Developer")]
        public IActionResult DeleteComment(int? commentId)
        {
            if(commentId != null)
            {
                try
                {
                    Comment commentToDelete = _db.Comment.First(comment => comment.Id == commentId);
                    _db.Remove(commentToDelete);
                    _db.SaveChanges();
                    return RedirectToAction("ViewTask", new {taskId = commentToDelete.ProjectTaskId});
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest("commentId was null");
            }
        }
        public IActionResult ViewNotifications(string? currentUserId)
        {
            if (currentUserId != null)
            {
                try
                {
                    List<Notification> notificationOfUser = _db.Notification
                        .Include(notification => notification.TargetUser)
                        .Where(notification => notification.TargetUserId == currentUserId).ToList();
                    return View(notificationOfUser);
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message + " At ViewNotifications");
                }
            }
            else
            {
                return BadRequest("currentUserId is null at ViewNotifications");
            }
        }
        public IActionResult DeleteNotification(int? notificationId)
        {
            if(notificationId != null)
            {
                try
                {
                    Notification notificationToDelete = _db.Notification.First(notification => notification.Id == notificationId);
                    _db.Remove(notificationToDelete);
                    _db.SaveChanges();
                    return RedirectToAction("ViewNotifications", new {currentUserId = notificationToDelete.TargetUserId});
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message + " At DeleteNotification");
                }
            }
            else
            {
                return BadRequest("notificationId is null at DeleteNotification");
            }
        }
        public IActionResult NotificationDetails(int? notificationId)
        {
            if (notificationId != null)
            {
                try
                {
                    Notification notification = _db.Notification
                        .Include(notification => notification.TargetUser)
                        .First(notification => notification.Id == notificationId);

                    return View(notification);
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message + " At NotificationDetails");
                }
            }
            else
            {
                return BadRequest("notificationId is null at NotificationDetails");
            }
        }
        public IActionResult MarkNotificationAsRead(int? notificationId)
        {
            if (notificationId != null)
            {
                try
                {
                    Notification notification = _db.Notification.First(notification => notification.Id == notificationId);
                    notification.IsRead = true;
                    _db.SaveChanges();

                    return RedirectToAction("NotificationDetails", new {notificationId = notificationId});
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message + " At MarkNotificationAsRead");
                }
            }
            else
            {
                return BadRequest("notificationId is null at MarkNotificationAsRead");
            }
        }


    }
}

/*
Create a User Manager class that has functions to manage users and roles 
(Get all roles for a user, assign roles to users, check if a user in a role)...etc

A Project manager will have a dashboard page which shows all the projects with their related tasks 
and assigned developers, the projects with high priorities appear first.

Developers can view all their tasks (but can’t view other developers’ tasks), 
also a developer can update a task to change the completed percentage of the task, or mark it totally completed, 
when the task is marked completed, the developer can leave a comment to describe any notes or hints`.

A developer will get a notification when the task is only one day to pass the deadline. 
The Developers should be able to see the number of notifications in their homepage, 
they can click on this number to go to a page where they can see all the notifications in detail.

-->The project manager can see a list of the tasks that are not finished yet and passed their deadline.

-->The project manager will get a notification if a project passed a deadline with any unfinished tasks.
-->A project manager gets a notification whenever a task or a project is completed.

Developers can leave an urgent Note to a task to mention a bug or a problem preventing them from completing the task, 
in this case a notification is sent to the Project Manager.

Add a new property to the notifications to determine if it is new or opened (unread).

Add a link called “Notifications” to the project manager dashboard which will take the manager to see all his notifications, 
this link also shows the number of current unopened notifications.
 */