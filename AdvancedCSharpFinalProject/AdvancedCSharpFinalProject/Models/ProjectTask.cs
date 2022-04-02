namespace AdvancedCSharpFinalProject.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public Priority Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Note> Notes { get; set; }

        public ProjectTask()
        {
            Notes = new List<Note>();
        }
    }
}
