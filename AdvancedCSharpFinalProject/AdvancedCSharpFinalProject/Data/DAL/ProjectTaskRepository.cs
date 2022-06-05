using System;
using AdvancedCSharpFinalProject.Data;
using AdvancedCSharpFinalProject.Data.DAL;
using AdvancedCSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCSharpFinalProject.Data.DAL
{
    public class ProjecTaskRespository : IRepository<ProjectTask>
    {
        private ApplicationDbContext _db { get; set; }

        public ProjecTaskRespository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ProjecTaskRespository()
        {

        }

        //comments, notes, developer?, project


        public virtual void Add(ProjectTask projectTask)
        {
            _db.ProjectTask.Add(projectTask);
        }

        public virtual ProjectTask Get(int id)
        {
            var ProjectTask = _db.ProjectTask.Include(pt => pt.Comments)
                                             .Include(pt => pt.Notes)
                                             .Include(pt => pt.Developer)
                                             .Include(pt => pt.Project).First(pt => pt.Id == id);
            return ProjectTask;
        }

        public virtual ICollection<ProjectTask> GetAll()
        {
            var ProjectTasks = _db.ProjectTask.Include(pt => pt.Comments)
                                             .Include(pt => pt.Notes)
                                             .Include(pt => pt.Developer)
                                             .Include(pt => pt.Project).ToList();
            return ProjectTasks;
        }

        public virtual ProjectTask Get(Func<ProjectTask, bool> firstFunction)
        {
            var ProjectTasks = _db.ProjectTask.Include(pt => pt.Comments)
                                             .Include(pt => pt.Notes)
                                             .Include(pt => pt.Developer)
                                             .Include(pt => pt.Project).ToList();
            return ProjectTasks.First(firstFunction);
        }

        public virtual ICollection<ProjectTask> GetList(Func<ProjectTask, bool> whereFunction)
        {
            var ProjectTasks = _db.ProjectTask.Include(pt => pt.Comments)
                                             .Include(pt => pt.Notes)
                                             .Include(pt => pt.Developer)
                                             .Include(pt => pt.Project);
            return ProjectTasks.Where(whereFunction).ToList();
        }

        public virtual void Update(ProjectTask projectTask)
        {
            _db.ProjectTask.Update(projectTask);
        }

        public virtual void Remove(ProjectTask projectTask)
        {
            _db.ProjectTask.Remove(projectTask);
        }

        public virtual void Save()
        {
            _db.SaveChanges();
        }
    }
}
