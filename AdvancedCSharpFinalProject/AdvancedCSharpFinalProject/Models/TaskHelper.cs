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
        public void UpdateTask(ProjectTask task, ProjectTask updatedTask)
        {
            task.Id = updatedTask.Id;
            task.DeveloperId = updatedTask.DeveloperId;
            task.ProjectId = updatedTask.ProjectId;
            task.Deadline = updatedTask.Deadline;
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Priority = updatedTask.Priority;

            if (updatedTask.IsCompleted == true)
            {
                task.IsCompleted = true;
                task.CompletionPercentage = 100;
                updatedTask.CompletionPercentage = 100;
            }
            else
            {
                task.CompletionPercentage = updatedTask.CompletionPercentage;
                task.IsCompleted = updatedTask.IsCompleted;
            }
            if (updatedTask.CompletionPercentage == 100)
            {
                updatedTask.IsCompleted = true;
                task.IsCompleted = true;
                task.CompletionPercentage = 100;
            }
            else
            {
                updatedTask.IsCompleted= false;
                task.CompletionPercentage = updatedTask.CompletionPercentage;
                task.IsCompleted = updatedTask.IsCompleted;
            }
        }

        public void DeleteTask(ApplicationDbContext db, ProjectTask task)
        {
            db.ProjectTask.Remove(task);
        }
    }
}

