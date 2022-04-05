using AdvancedCSharpFinalProject.Data;
using Microsoft.AspNetCore.Authorization;

namespace AdvancedCSharpFinalProject.Models
{
    [Authorize(Roles = "Project Manager")]
    public class TaskHelper
    {
        public TaskHelper() { }
        public void AddTask(ApplicationDbContext db, ProjectTask task)
        {
            db.ProjectTask.Add(task);
            db.SaveChanges();
        }

        public void AddDeveloperToTask(ApplicationDbContext db, ProjectTask task, Developer developer)
        {
            task.Developer = developer;
            db.Update(task);
            db.SaveChanges();
        }

        public void DeleteTask(ApplicationDbContext db, ProjectTask task)
        {
            db.ProjectTask.Remove(task);
            db.SaveChanges();
        }
    }
}

