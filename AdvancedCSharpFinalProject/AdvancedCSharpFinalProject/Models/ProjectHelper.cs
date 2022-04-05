using Microsoft.AspNetCore.Identity;

namespace AdvancedCSharpFinalProject.Models
{
    public class ProjectHelper
    {
        public UserManager<ApplicationUser> _userManager { get; set; }
        public ApplicationUser ProjectManager { get; set; }
        public Project AddProject(Project newProject)
        {
            ProjectManager projectManager = new ProjectManager(ProjectManager);

            newProject.ProjectManager = projectManager; //only accepts ProjectManager object not ApplicationUser
            newProject.ProjectManagerId = projectManager.Id;
            ProjectManager.Projects.Add(newProject);

            newProject.ProjectTasks = new HashSet<ProjectTask>();
            newProject.IsCompleted = false;
            newProject.CompletionPercentage = 0;
            newProject.ActualBudget = 0;

            return newProject;
        }
        public ProjectHelper(UserManager<ApplicationUser> userManager, ApplicationUser projectManager)
        {
            _userManager = userManager;
            ProjectManager = projectManager;
        }
    }
}
/*
Create a User Manager class that has functions to manage users and roles 
(Get all roles for a user, assign roles to users, check if a user in a role)...etc

Create a ProjectHelper class that contains functions to add, delete, update projects, 
along with any other helper functions. 
Those functions can only be accessed by project managers.
*/