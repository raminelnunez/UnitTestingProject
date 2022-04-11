using System.ComponentModel.DataAnnotations;

namespace AdvancedCSharpFinalProject.Models
{
    public class Comment //breakTable between ProjectTask and Developer
    {
        public int Id { get; set; }
        public ApplicationUser Developer { get; set; }
        public string DeveloperId { get; set; }
        public ProjectTask ProjectTask { get; set; }
        public int ProjectTaskId { get; set; }
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Title has to be more than 1 and less than 1000 characters")]
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
//when the task is marked completed, the developer can leave a comment to describe any notes or hints`.
