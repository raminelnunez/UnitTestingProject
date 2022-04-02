namespace AdvancedCSharpFinalProject.Models
{
    public class Note // a note 
    {
        public int Id { get; set; }
        public Developer Developer { get; set; }
        public string DeveloperId { get; set; }
        public ProjectTask ProjectTask { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
    }
}
