namespace AdvancedCSharpFinalProject.Models
{
    public class Project
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public Priority Priority { get; set; }
        public int CompletionPercentage { get; set; }
        public DateTime Deadline { get; set; }
        public ProjectManager ProjectManager { get; set; }
        public List<ProjectTask> Tasks { get; set; }
        public List<Developer> Developers { get; set; }
    }
}

public enum Priority
{
    Low,
    Medium,
    High,
}
