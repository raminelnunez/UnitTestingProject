namespace AdvancedCSharpFinalProject.Models
{
    public class Project
    {
        public bool isCompleted { get; set; }
        public enum Priority { Low, Medium, High}
        public int CompletionPercentage { get; set; }
        public DateTime Deadline { get; set; }
        public ProjectManager ProjectManager { get; set; }
        public List<ProjectTask> Tasks { get; set; }
        public List<Developer> Developers { get; set; }
    }
}
