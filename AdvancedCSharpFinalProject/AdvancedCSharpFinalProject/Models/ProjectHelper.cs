using AdvancedCSharpFinalProject.Data;
using Microsoft.AspNetCore.Identity;

namespace AdvancedCSharpFinalProject.Models
{
    public class ProjectHelper
    {
        public ApplicationDbContext db { get; set; }
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
        public void UpdateProject(Project project, Project updatedProject)
        {
            //values that can be changed
            project.Title = updatedProject.Title;
            project.ActualBudget = updatedProject.ActualBudget;
            project.AssignedBudget = updatedProject.AssignedBudget;
            project.Deadline = updatedProject.Deadline;
            project.IsCompleted = updatedProject.IsCompleted;
            project.Priority = updatedProject.Priority;
            if(updatedProject.IsCompleted == true)
            {
                project.CompletionPercentage = 100;
            }
            else
            {
                project.CompletionPercentage = updatedProject.CompletionPercentage;
            }
            if(updatedProject.CompletionPercentage == 100)
            {
                project.IsCompleted = true;
            }
            else
            {
                project.IsCompleted = updatedProject.IsCompleted;
            }
        }
        public void DeleteProject(Project projectToDelete)
        {
            db.Project.Remove(projectToDelete);
        }
        public ProjectHelper(ApplicationUser projectManager)
        {
            ProjectManager = projectManager;
        }
        public ProjectHelper(ApplicationDbContext _db)
        {
            db = _db;
        }
        public ProjectHelper()
        {

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