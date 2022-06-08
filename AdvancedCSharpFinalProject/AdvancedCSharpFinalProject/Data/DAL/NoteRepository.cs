using System;
using AdvancedCSharpFinalProject.Data;
using AdvancedCSharpFinalProject.Data.DAL;
using AdvancedCSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCSharpFinalProject.Data.DAL
{
    public class NoteRepository : IRepository<Note>
    {
        private ApplicationDbContext _db { get; set; }

        public NoteRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public NoteRepository()
        {

        }

        //projectTask, Developer

        public virtual void Add(Note note)
        {
            _db.Note.Add(note);
        }
        public virtual Note Get(int id)
        {
            var Note = _db.Note.Include(n => n.ProjectTask).Include(n => n.Developer).First(x => x.Id == id);
            return Note;
        }

        public virtual ICollection<Note> GetAll()
        {
            var Notes = _db.Note.Include(n => n.ProjectTask)
                                             .Include(n => n.Developer)
                                             .ToList();
            return Notes;
        }

        public virtual Note Get(Func<Note, bool> firstFunction)
        {
            var Notes = _db.Note.Include(n => n.ProjectTask)
                                             .Include(n => n.Developer)
                                             .ToList();
            return Notes.First(firstFunction);
        }

        public virtual ICollection<Note> GetList(Func<Note, bool> whereFunction)
        {
            var Notes = _db.Note.Include(n => n.ProjectTask).Include(n => n.Developer);

            return Notes.Where(whereFunction).ToList();
        }

        public virtual void Update(Note note)
        {
            _db.Note.Update(note);
        }

        public virtual void Remove(Note note)
        {
            _db.Note.Remove(note);
        }

        public virtual void Save()
        {
            _db.SaveChanges();
        }
    }
}
