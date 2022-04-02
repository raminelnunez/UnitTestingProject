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
        public List<ProjectTask> Tasks { get; set; }
        public List<Developer> Developers { get; set; }
        public void CalculateActualBudget()
        {
            Developers.ForEach(developer => ActualBudget += developer.DailySalary);
        }
    }
}

public enum Priority
{
    Low,
    Medium,
    High,
}
