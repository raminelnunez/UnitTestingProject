using System;
using AdvancedCSharpFinalProject.Data.DAL;
using AdvancedCSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCSharpFinalProject.Data.BLL
{
	public class NotificationBusinessLogic
	{
		public NotificationRepository NotificationRepo;

        public NotificationBusinessLogic(NotificationRepository repository)
			{
				NotificationRepo = repository;
			}

		public NotificationBusinessLogic()
        {
			
        }

		public void LoadNotifications(Project project, ApplicationUser currentUser, ProjectTask task)
        {
            if (task.IsCompleted == false && project.IsNotified == false)
            {
                Notification notification = new Notification(currentUser, $"Project: <b style=\"color:purple\">{project.Title}</b> passed it's deadline with an unfinished Task: <b style=\"color:purple\">{task.Title}</b>");
                project.IsNotified = true;
                currentUser.Notifications.Add(notification);
                NotificationRepo.Add(notification);
                NotificationRepo.Save();
            }
        }

        public Notification GetNotificationById(int? notificationId)
        {
            if (notificationId != null)
            {
                try
                {
                    Notification? notification = NotificationRepo.Get((int)notificationId);
                    if (notification != null)
                        return NotificationRepo.Get((int)notificationId);
                    else
                        throw new Exception("Notification was not found.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Notification was not found.");
                }
            }
            else
            {
                throw new Exception("notification id is null");
            }
            
        }

        public ICollection<Notification> GetAllNotifications()
        {
            var AllNotifications = new List<Notification>();
            try
            {
                AllNotifications = (List<Notification>)NotificationRepo.GetAll();
            }
            catch
            {
                throw new Exception("No notifications found");
            }

            return (ICollection<Notification>)AllNotifications;
        }

        public ICollection<Notification> GetUserNotifications(ApplicationUser user)
        {
            if (user.Id != null)
            {
                try
                {
                    var AllNotifications = NotificationRepo.GetList(n => n.TargetUserId == user.Id);
                    return AllNotifications;
                }
                catch
                {
                    throw new Exception("No user was found.");
                }
            }
            else
            {
                throw new Exception("userId input is null");
            }
        }

        public ICollection<Notification> GetUserNotificationsById(string? userId)
        {
            if (userId != null)
            {
                try
                {
                    var AllNotifications = NotificationRepo.GetList(n => n.TargetUserId == userId);
                    return AllNotifications;
                }
                catch
                {
                    throw new Exception("No user was found.");
                }
            }
            else
            {
                throw new Exception("userId input is null");
            }
        }

        public void CreateNotification(ApplicationUser targetUser, string message)
        {
            Notification notification = new Notification();
            if (targetUser != null && message != null)
            {
                try
                {
                    notification = new Notification(targetUser, message);
                    NotificationRepo.Add(notification);
                    NotificationRepo.Save();
                }
                catch
                {
                    throw new Exception("The notification could not be created");
                }
            }
            else
            {
                throw new Exception("One of your parameters is null");
            }

        }

        public Notification CreateAndReturnNotification(ApplicationUser targetUser, string message)
        {
            Notification notification = new Notification();
            if (targetUser != null && message != null)
            {
                try
                {
                    notification = new Notification(targetUser, message);
                    NotificationRepo.Add(notification);
                    NotificationRepo.Save();
                    return notification;
                }
                catch
                {
                    throw new Exception("The notification could not be created");
                }
            }
            else
            {
                throw new Exception("One of your parameters is null");
            }
        }

        public void DeleteNotificationById(int? notificationId)
        {
            if (notificationId == null)
            {
                try
                {
                    Notification notificationToDelete = GetNotificationById((int)notificationId);
                    NotificationRepo.Remove(notificationToDelete);
                    NotificationRepo.Save();
                }
                catch
                {
                    throw new Exception("The notification could not be deleted or does not exist");
                }
            } else
            {
                throw new Exception("notification id is null");
            }
            
        }

        public void DeleteNotification(Notification notificationToDelete)
        {
            NotificationRepo.Remove(notificationToDelete);
            NotificationRepo.Save();
        }

        public void UpdateNotification(Notification notificationToUpdate)
        {
            NotificationRepo.Update(notificationToUpdate);
            NotificationRepo.Save();
        }

    }
}

