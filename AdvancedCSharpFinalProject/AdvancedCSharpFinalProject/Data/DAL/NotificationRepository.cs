using System;
using AdvancedCSharpFinalProject.Data.DAL;
using AdvancedCSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCSharpFinalProject.Data
{
	public class NotificationRepository : IRepository<Notification>
	{
        private ApplicationDbContext _db { get; set; }

        // Constructors
        public NotificationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public NotificationRepository()
        {

        }

        // CRUD

        // CREATE
        public virtual void CreateProject(Notification notification)
        {
            _db.Notification.Add(notification);
        }

        // READ
        public virtual Notification Get(int id)
        {
            var Notification = _db.Notification.First(n => n.Id == id);
            Notification.TargetUser = _db.Users.First(u => u.Id == Notification.TargetUserId);
            return Notification;
        }

        public virtual ICollection<Notification> GetAll()
        {
            var Notifications = _db.Notification.Include(n => n.TargetUser);
            return Notifications.ToList();
        }

        public virtual Notification Get(Func<Notification, bool> firstFunction)
        {
            var Notifications = _db.Notification.Include(n => n.TargetUser);
            return Notifications.First(firstFunction);
        }

        public virtual ICollection<Notification> GetList(Func<Notification, bool> whereFunction)
        {
            var Notifications = _db.Notification.Include(n => n.TargetUser);
            return Notifications.Where(whereFunction).ToList();
        }

        // UPDATE
        public virtual void Update(Notification notification)
        {
            _db.Notification.Update(notification);
        }

        // DELETE
        public virtual void Remove(Notification notification)
        {
            _db.Notification.Remove(notification);
        }

        // SAVE
        public virtual void Save()
        {
            _db.SaveChanges();
        }
    }
}

