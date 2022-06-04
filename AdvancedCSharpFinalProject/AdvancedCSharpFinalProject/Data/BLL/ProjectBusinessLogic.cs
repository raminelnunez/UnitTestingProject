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
        public NotificationBusinessLogic TicketBLL { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }

        // CONSTRUCTORS
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


        // GET LOGIC
        public Project GetProjectById(int id)
        {
            try
            {
                Project project = ProjectRepo.Get(id);
                if(project != null)
                    return ProjectRepo.Get(id);
                else
                    throw new Exception("Project was not found.");
            }
            catch (Exception ex)
            {
                throw new Exception("Project was not found.");
            }
        }

        public ICollection<Project> GetAllProjects()
        {
            var AllProjects = ProjectRepo.GetAll();

            return AllProjects;
        }

        public ICollection<Project> GetCurrentProjects(ApplicationUser user)
        {
            if(user != null)
            {
                try
                {
                    var AllProjects = ProjectRepo.GetList(p => p.Users.Contains(user));
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


        // POST LOGIC
        public void CreateProject(string title, string description)
        {
            if (title != null && description != null)
            {
                try
                {
                    Project newProject = new Project()
                    {
                        Title = title,
                        Description = description,
                        Users = new List<ApplicationUser>()
                    };

                    ProjectRepo.CreateProject(newProject);
                    ProjectRepo.Save();
                }
                catch
                {
                    throw new Exception("The project could not be created");
                }
            }
            else if (title == null || description == null)
            {
                throw new Exception("Title and Description must be filled out.");
            }
   
        }

        public void EditProject(int id, string? title, string? description)
        {
            if (id != null)

            {
                try
                {
                    var ProjectToEdit = ProjectRepo.Get(id);
                    ProjectToEdit.Title = title;
                    ProjectToEdit.Description = description;

                    ProjectRepo.Update(ProjectToEdit);
                    ProjectRepo.Save();
                }
                catch (Exception ex)
                {
                    throw new Exception("The project could not be updated.");
                }
            } 
            else if (id == null)
            {
                throw new Exception("Project was not found.");
            }
            else if (title == null && description == null)
            {
                throw new Exception("Title or Description must be filled out.");
            }

        }

        public void AssignTicket(string devId, Ticket Ticket)
        {
            if (Ticket != null && devId != null)
            {
                try
                {
                    Ticket.UserId = devId;
                    Ticket.ticketStatus = TicketStatus.Assigned;

                    ProjectRepo.Save();
                }
                catch (Exception ex)
                {
                    throw new Exception("The Developer could not be assigned.");
                }
            }
            else if(Ticket == null)
            {
                throw new Exception("The Ticket could not be found.");
            }
            else if(devId == null)
            {
                throw new Exception("The Developer you chose could not be found.");
            }
        }

        public void AssignProject(AppUser pm, int projectId)
        {
            if (projectId != null && pm != null)
            {
                try
                {
                    Project project = GetProjectById(projectId);

                    project.Users.Add(pm);
                    ProjectRepo.Save();
                }
                catch
                {
                    throw new Exception("The Manager could not be assgined.");
                }
            }
            else if (pm == null)
            {
                throw new Exception("The user was not found.");
            }
            else if (projectId == null)
            {
                throw new Exception("The project was not found.");
            }
   
        }

    }
}
