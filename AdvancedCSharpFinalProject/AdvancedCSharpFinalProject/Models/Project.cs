namespace AdvancedCSharpFinalProject.Models
{
    public class Project
    {
        public int Id { get; set; }
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
        public Project(ProjectManager projectManager, double assignedBudget, Priority priority, DateTime deadline)
        {
            ProjectManager = projectManager;
            ProjectManagerId = projectManager.Id;
            AssignedBudget = assignedBudget;
            Priority = priority;
            Deadline = deadline;
            ProjectTasks = new HashSet<ProjectTask>();
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
