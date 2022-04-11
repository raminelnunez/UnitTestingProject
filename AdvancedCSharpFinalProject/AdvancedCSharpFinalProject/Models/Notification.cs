using System.ComponentModel.DataAnnotations;

namespace AdvancedCSharpFinalProject.Models
{
    public class Notification//One To Many between ApplicationUser and Notification
    {
		public int Id { get; set; }
		[Display(Name = "User")]
		public ApplicationUser TargetUser { get; set; }
		public string TargetUserId { get; set; }
		public string Message { get; set; }
		[Display(Name = "View Status")]
		public bool IsRead { get; set; }
		public Notification() 
		{ 
		}
		public Notification(ApplicationUser targetUser, string message)
		{
			TargetUser = targetUser;
			TargetUserId = targetUser.Id;
			Message = message;
			IsRead = false;
		}
	}
}
