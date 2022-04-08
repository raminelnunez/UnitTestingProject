using System.ComponentModel.DataAnnotations;

namespace AdvancedCSharpFinalProject.Models
{
    public class ProjectTask //break between Project and Developer
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public ApplicationUser? Developer { get; set; }
        public string? DeveloperId { get; set; }
        public Priority Priority { get; set; }
        [Display(Name = "Completion Status")]
        public bool IsCompleted { get; set; }
        [Display(Name = "Completion % of Task")]
        public int CompletionPercentage { get; set; }
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Title has to be more than 3 and less than 200 characters")]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }

        public ProjectTask(Project project, string title, string description, DateTime deadline, Priority priority)
        {
            Project = project;
            ProjectId = project.Id;
            Title = title;
            Description = description;
            Deadline = deadline;
            Priority = priority;
        }

        public ProjectTask(Project project, ApplicationUser? developer, string title, string description, DateTime deadline, Priority priority)
        {
            Project = project;
            ProjectId = project.Id;
            Developer = developer;
            DeveloperId = developer.Id;
            Title = title;
            Description = description;
            Deadline = deadline;
            Priority = priority;
        }

        public ProjectTask()
        {
        }
    }
}
