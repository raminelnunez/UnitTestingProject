namespace AdvancedCSharpFinalProject.Models
{
    public class Note
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Task Task { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
    }
}
