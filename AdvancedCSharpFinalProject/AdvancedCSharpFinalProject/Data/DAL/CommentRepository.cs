using System;
using AdvancedCSharpFinalProject.Data;
using AdvancedCSharpFinalProject.Data.DAL;
using AdvancedCSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCSharpFinalProject.Data.DAL
{
    public class CommentRepository : IRepository<Comment>
    {
        private ApplicationDbContext _db { get; set; }

        public CommentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public CommentRepository()
        {

        }

        //projectTask, Developer

        public virtual void Add(Comment comment)
        {
            _db.Comment.Add(comment);
        }

        public virtual Comment Get(int id)
        {
            var comment = _db.Comment.Include(c => c.Developer).First(x => x.Id == id);
            return comment;
        }

        public virtual ICollection<Comment> GetAll()
        {
            var comments = _db.Comment.Include(c => c.Developer)
                                             .ToList();
            return comments;
        }

        public virtual Comment Get(Func<Comment, bool> firstFunction)
        {
            var comments = _db.Comment.Include(c => c.Developer)
                                            .ToList();
            return comments.First(firstFunction);
        }

        public virtual ICollection<Comment> GetList(Func<Comment, bool> whereFunction)
        {
            var comments = _db.Comment.Include(c => c.Developer);

            return comments.Where(whereFunction).ToList();
        }

        public virtual void Update(Comment comment)
        {
            _db.Comment.Update(comment);
        }

        public virtual void Remove(Comment comment)
        {
            _db.Comment.Remove(comment);
        }

        public virtual void Save()
        {
            _db.SaveChanges();
        }
    }
}
