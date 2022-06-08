using System.ComponentModel.DataAnnotations;
namespace AdvancedCSharpFinalProject.Models
{
    public class Project
    {
        public int Id { get; set; }
        [StringLength(200, MinimumLength = 3, ErrorMessage ="Title has to be more than 3 and less than 200 characters")]
        public string Title { get; set; }
        [Display(Name ="Completion Status")]
        public bool IsCompleted { get; set; }
        public Priority Priority { get; set; }
        [Display(Name = "Completion %")]
        public float CompletionPercentage { get; set; }
        public DateTime Deadline { get; set; }
        [Display(Name = "Assigned Budget")]
        public double AssignedBudget { get; set; }
        [Display(Name = "Actual Budget")]
        public double ActualBudget { get; set; }
        [Display(Name = "Project Manager")]
        public ApplicationUser ProjectManager { get; set; }
        public string ProjectManagerId { get; set; }
        public ICollection<ProjectTask> ProjectTasks { get; set; }
        public bool IsNotified { get; set; }
        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }
        public void CalculateActualBudget()
        {
            double totalDaysWork = (DateTime.Now - CreatedDate).TotalDays;
            foreach(ProjectTask task in ProjectTasks)
            {
                if(task.Developer != null)
                {
                    double totalIncomeForDeveloper = task.Developer.DailySalary * totalDaysWork;
                    ActualBudget += totalIncomeForDeveloper;
                }
            }
        }
        public Project(ApplicationUser projectManager, string title, double assignedBudget, Priority priority, DateTime deadline)
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
    Low = 0,
    Medium = 1,
    High = 2,
}
