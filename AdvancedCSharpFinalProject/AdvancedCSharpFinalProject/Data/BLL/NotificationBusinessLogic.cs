﻿using System;
using AdvancedCSharpFinalProject.Data.DAL;
using AdvancedCSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCSharpFinalProject.Data.BLL
{
	public class NotificationBusinessLogic
	{
		public NotificationRepository repo;

        public NotificationBusinessLogic(NotificationRepository repository)
			{
				repo = repository;
			}

		public NotificationBusinessLogic()
        {
			
        }

		public List<Ticket> GetAll()
			{
				return repo.GetAll().ToList();
			}
		public void AddTicket(Ticket ticket)
        {
			repo.Add(ticket);
			repo.Save();
        }

		public TicketComment AddComment(int id, string comment, DateTime createdDate, AppUser user)
        {
			Ticket ticket = repo.Get(id);
			if(ticket == null || id == null || comment == null || comment == string.Empty)
            {
				throw new Exception("Please input correct values");
			}
			TicketComment NewComment = new TicketComment
			{
				Body = comment,
				CreatedDate = createdDate,
				Ticket = ticket,
				TicketId = ticket.Id,
				User = user,
				UserId = user.Id
			};
			ticket.TicketComments.Add(NewComment);
			repo.Save();
			return NewComment;
		}

		public List<TicketComment> ViewComments(Ticket ticket)
        {
			return ticket.TicketComments.ToList();
        }

		public Ticket Get(int id)
        {
			if (id == null)
			{
				throw new Exception("Ticket Id does not exist");
			}
			Ticket ticket = repo.Get(id); 
			if(ticket == null)
            {
				throw new Exception("Ticket not found");
			}
			return repo.Get(id);
        }

		public void Edit(Ticket ticket, string Title, string Description, DateTime CreatedDate, DateTime UpdatedDate, TicketStatus ticketStatus, TicketType ticketType, TicketPriority ticketPriority)
		{
			if (ticket == null)
			{
				throw new Exception("Ticket not found");
			}
			try
			{
				ticket.Title = Title;
				ticket.Description = Description;
				ticket.CreatedDate = CreatedDate;
				ticket.UpdatedDate = UpdatedDate;
				ticket.ticketStatus = ticketStatus;
				ticket.ticketType = ticketType;
				ticket.ticketPriority = ticketPriority;

				repo.Update(ticket);
				repo.Save();
			}
			catch (DbUpdateConcurrencyException)
			{
					throw;
			}

		}

		public ICollection<Ticket> SubmitterTickets(string submitterId)
		{
			List<Ticket> Tickets = (List<Ticket>)repo.GetAll().Where(t => t.SubmitterId == submitterId).ToList();
			return Tickets;
		}

		public ICollection<Ticket> DeveloperAssignedTickets(string devID)
		{
			List<Ticket> Tickets = (List<Ticket>)repo.GetAll().Where(t => t.UserId == devID).ToList();
			return Tickets;
		}

		public void MarkTicketAsResolved(int id)
        {
			Ticket ticketToResolve = repo.Get(id);
			ticketToResolve.ticketStatus = TicketStatus.Resolved;
			repo.Save();
        }

		public void DeleteTicket(int id)
        {
			if (id == null)
			{
				throw new ArgumentException("Ticket Id does not exist");
			}
			repo.Remove(repo.Get(id));
			repo.Save();
        }
    }
}

