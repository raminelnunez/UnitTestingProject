namespace AdvancedCSharpFinalProject.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public enum Priority { Low, Medium, High }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Note> Notes { get; set; }

        public ProjectTask()
        {
            Notes = new List<Note>();
        }
    }
}
