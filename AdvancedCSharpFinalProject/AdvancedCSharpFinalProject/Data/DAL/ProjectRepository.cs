using System;
using AdvancedCSharpFinalProject.Data;
using AdvancedCSharpFinalProject.Data.DAL;
using AdvancedCSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCSharpFinalProject.Data.DAL
{
    public class ProjectRepository : IRepository<Project>
    {
        private ApplicationDbContext _db { get; set; }

        // Constructors
        public ProjectRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ProjectRepository()
        {

        }

        public virtual void Add(Project project)
        {
            _db.Project.Add(project);
        }

        public virtual Project Get(int id)
        {
            var Project = _db.Project.First(p => p.Id == id);
            var ProjectTasks = _db.ProjectTask.Where(p => p.ProjectId == id);
            Project.ProjectTasks = ProjectTasks.ToList();
            return Project;
        }

        public virtual Project GetProjectForCreateTask(int ProjectId)
        {
            Project ProjectToReturn =_db.Project
                    .Include(project => project.ProjectTasks)
                    .Include(project => project.ProjectManager)
                    .Include(project => project.ProjectTasks)
                    .ThenInclude(task => task.Comments)
                    .Include(project => project.ProjectTasks)
                    .ThenInclude(task => task.Notes)
                    .ThenInclude(note => note.Developer)
                    .First(project => project.Id == ProjectId);
            return ProjectToReturn;
        }
        public virtual ICollection<Project> GetAll()
        {
            var Projects = _db.Project.Include(p => p.ProjectManager).Include(p => p.ProjectTasks).ThenInclude(t => t.Developer);
            return Projects.ToList();
        }

        public virtual Project Get(Func<Project, bool> firstFunction)
        {
            var Projects = _db.Project.Include(p => p.ProjectManager).Include(p => p.ProjectTasks).ThenInclude(t => t.Developer);
            return Projects.First(firstFunction);
        }

        public virtual ICollection<Project> GetList(Func<Project, bool> whereFunction)
        {
            var Projects = _db.Project.Include(p => p.ProjectManager).Include(p => p.ProjectTasks).ThenInclude(t => t.Developer);
            return Projects.Where(whereFunction).ToList();
        }

        public virtual void Update(Project project)
        {
            _db.Project.Update(project);
        }

        public virtual void Remove(Project project)
        {
            _db.Project.Remove(project);
        }

        public virtual void Save()
        {
            _db.SaveChanges();
        }
    }
}
