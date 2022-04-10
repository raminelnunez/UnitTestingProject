using System.ComponentModel.DataAnnotations;

namespace AdvancedCSharpFinalProject.Models
{
    public class Note 
    {
        //-->Developers can leave an urgent Note to a task to mention a bug or a problem preventing them from completing the task,
        //in this case a notification is sent to the Project Manager.

        public int Id { get; set; }
        public ApplicationUser Developer { get; set; }
        public string DeveloperId { get; set; }
        public ProjectTask ProjectTask { get; set; }
        public int ProjectTaskId { get; set; }
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "note has to be more than 1 and less than 1000 characters")]
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
