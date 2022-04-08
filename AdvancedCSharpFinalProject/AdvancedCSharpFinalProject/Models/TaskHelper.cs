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
        }

        public void AddDeveloperToTask(ApplicationDbContext db, ProjectTask task, ApplicationUser developer)
        {
            task.Developer = developer;
            task.DeveloperId = developer.Id;
            developer.ProjectTasks.Add(task);
            db.Update(task);
        }

        public void DeleteTask(ApplicationDbContext db, ProjectTask task)
        {
            db.ProjectTask.Remove(task);
            db.SaveChanges();
        }
    }
}

