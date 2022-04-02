namespace AdvancedCSharpFinalProject.Models
{
    public class ProjectTask //break between Project and Developer
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public Developer Developer { get; set; }
        public string DeveloperId { get; set; }
        public Priority Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Note> Notes { get; set; }
        public DateTime Deadline { get; set; }

        public ProjectTask(Project project, Developer developer, string title, string description, DateTime deadline, Priority priority)
        {
            Project = project;
            ProjectId = project.Id;
            Developer = developer;
            DeveloperId = developer.Id;
            Title = title;
            Description = description;
            Deadline = deadline;
            Priority = priority;
            Notes = new HashSet<Note>();
        }

        public ProjectTask()
        {
        }
    }
}
