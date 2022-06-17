using System;
using System.Linq;
using AdvancedCSharpFinalProject.Data;
using AdvancedCSharpFinalProject.Data.DAL;
using AdvancedCSharpFinalProject.Models;
using Microsoft.AspNetCore.Identity;

namespace AdvancedCSharpFinalProject.Data.BLL
{
    public class ProjectBusinessLogic
    {
        public ProjectRepository ProjectRepo { get; set; }
        public NotificationRepository NotificationRepo { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }

        public ProjectBusinessLogic(ProjectRepository repo)
        {
            ProjectRepo = repo;
        }

        public ProjectBusinessLogic(ProjectRepository repo, UserManager<ApplicationUser> userManager)

        {
            ProjectRepo = repo;
            _userManager = userManager;
        }

        public ProjectBusinessLogic()
        {
            
        }


        public Project GetProjectById(int? projectId)
        {
            if (projectId != null)
            {
                try
                {
                    Project? project = ProjectRepo.Get((int)projectId);
                    if (project != null)
                        return ProjectRepo.Get((int)projectId);
                    else
                        throw new Exception("Project was not found.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Project was not found.");
                }
            }
            else
            {
                throw new Exception("project id is null");
            }
            
        }

        public virtual Project GetProjectForUpdateProject(int? ProjectId)
        {
            if (ProjectId != null)
            {
                try
                {
                    return ProjectRepo.GetProjectForUpdateProject((int)ProjectId);
                }
                catch
                {
                    throw new Exception("project not found");
                }
            }
            else
            {
                throw new Exception("project id is null");
            }
            
        }
        public ICollection<Project> GetAllProjects()
        {
            var AllProjects = new List<Project>();
            try
            {
                AllProjects = (List<Project>)ProjectRepo.GetAll();
                return (ICollection<Project>)AllProjects;
            }
            catch
            {
                throw new Exception("No projects found");
            }

            
        }

        public ICollection<Project> GetCurrentProjects(ApplicationUser user)
        {
            if(user != null)
            {
                try
                {
                    var AllProjects = ProjectRepo.GetList(p => p.ProjectManager == user);
                    return AllProjects;
                }
                catch
                {
                    throw new Exception("No user was found.");
                }
            }
            else
            {
                throw new Exception("No user was found.");
            }
        }


        
        public void CreateProject(ApplicationUser projectManager, string title, double assignedBudget, Priority priority, DateTime deadline)
        {
            Project newProject = new Project();
            if (projectManager != null && assignedBudget != null && title != null && priority != null && deadline != null)
            {
                try
                {
                    newProject = new Project(projectManager, title, assignedBudget, priority, deadline);

                    ProjectRepo.Add(newProject);
                    ProjectRepo.Save();
                }
                catch
                {
                    throw new Exception("The project could not be created");
                }
            }
            else
            {
                throw new Exception("One of your parameters is null");
            }
   
        }

        public void EditProject(int projectId, string? title, double? assignedBudget, Priority? priority, DateTime? deadline)
        {
            Project project = GetProjectForUpdateProject(projectId);
            try
            {
                if (title != null)
                {
                    project.Title = title;
                }
                if (assignedBudget != null)
                {
                    project.AssignedBudget = (double)assignedBudget;
                }
                if (priority != null)
                {
                    project.Priority = (Priority)priority;
                }
                if (deadline != null)
                {
                    project.Deadline = (DateTime)deadline;
                }
                Update(project);
            } catch
            {

            }
        }

        public Project GetProjectForCreateTask(int? ProjectId)
        {
            if (ProjectId != null)
            {
                Project project = new Project();
                try
                {
                    project = ProjectRepo.GetProjectForCreateTask((int)ProjectId);
                }
                catch
                {
                    throw new Exception("Could not get project");
                }
                return project;
            }
            else
            {
                throw new Exception("project id is null");
            }
        }

        public void Update(Project project)
        {
            try
            {
                ProjectRepo.Update(project);
                ProjectRepo.Save();
            } catch
            {
                throw new Exception("Project wasn't updated");
            }
        }

        public void RemoveProject(Project project)
        {
            if (project != null)
            {
                try
                {
                    ProjectRepo.Remove(project);

                }
                catch (Exception ex)
                {
                    throw new Exception("Project not deleted");
                }
            }
            else
            {
                throw new Exception("project input is null");
            }
        }
        public void RemoveProjectById(int? projectId)
        {
            if (projectId != null)
            {
                try
                {
                    Project projectToDelete = GetProjectById(projectId);
                    ProjectRepo.Remove(projectToDelete);
                    
                }
                catch (Exception ex)
                {
                    throw new Exception("Project not deleted");
                }
            }
            else
            {
                throw new Exception("project Id is null");
            }
        }

        public List<Project> GetProjectsWhere(Func<Project, bool> whereFunction)
        {
            List<Project> projectsToReturn = new List<Project>();
            try
            {
                projectsToReturn = (List<Project>)ProjectRepo.GetList(whereFunction);
            }
            catch
            {

            }
            if (projectsToReturn.Count < 1)
            {
                throw new Exception("no projects found");
            }
            return projectsToReturn;
        }

    }
}
