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

        // CRUD

        // CREATE
        public virtual void CreateProject(Project project)
        {
            _db.Project.Add(project);
        }

        // READ
        public virtual Project Get(int id)
        {
            var Project = _db.Project.First(p => p.Id == id);
            var ProjectTasks = _db.ProjectTask.Where(p => p.ProjectId == id);
            Project.ProjectTasks = ProjectTasks.ToList();
            return Project;
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

        // UPDATE
        public virtual void Update(Project project)
        {
            _db.Project.Update(project);
        }

        // DELETE
        public virtual void Remove(Project project)
        {
            _db.Project.Remove(project);
        }

        // SAVE
        public virtual void Save()
        {
            _db.SaveChanges();
        }
    }
}
