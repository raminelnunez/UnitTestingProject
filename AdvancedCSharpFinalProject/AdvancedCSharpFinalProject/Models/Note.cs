namespace AdvancedCSharpFinalProject.Models
{
    public class Note
    {
        public ApplicationUser User { get; set; }
        public Task Task { get; set; }
        public string Description { get; set; }
    }
}
