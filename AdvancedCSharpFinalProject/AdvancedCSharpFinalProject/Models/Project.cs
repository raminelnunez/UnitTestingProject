using System.ComponentModel.DataAnnotations;
namespace AdvancedCSharpFinalProject.Models
{
    public class Project
    {
        public int Id { get; set; }
        [StringLength(200, MinimumLength = 3, ErrorMessage ="Title has to be more than 3 and less than 200 characters")]
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public Priority Priority { get; set; }
        public float CompletionPercentage { get; set; }
        public DateTime Deadline { get; set; }
        public double AssignedBudget { get; set; }
        public double ActualBudget { get; set; }
        public ProjectManager ProjectManager { get; set; }
        public string ProjectManagerId { get; set; }
        public ICollection<ProjectTask> ProjectTasks { get; set; }
        public void CalculateActualBudget()
        {
            ProjectTasks.ToList().ForEach(projectTask => ActualBudget += projectTask.Developer.DailySalary);
        }
        public Project(ProjectManager projectManager, string title, double assignedBudget, Priority priority, DateTime deadline)
        {
            ProjectManager = projectManager;
            ProjectManagerId = projectManager.Id;
            Title = title;
            AssignedBudget = assignedBudget;
            Priority = priority;
            Deadline = deadline;
            ProjectTasks = new HashSet<ProjectTask>();
            IsCompleted = false;
            CompletionPercentage = 0;
            ActualBudget = 0;
        }
        public Project()
        {

        }
    }
}

public enum Priority
{
    Low,
    Medium,
    High,
}
