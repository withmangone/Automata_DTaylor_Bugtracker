using Automata_DTaylor_Bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void TriggerAssignmentNotifications(int ticketId, string oldDeveloper, string newDeveloper)
        {
            //set up a few simple variables that tell me which situation is afoot
            var newAssignment = string.IsNullOrEmpty(oldDeveloper) && !string.IsNullOrEmpty(newDeveloper);
            var unAssignment = !string.IsNullOrEmpty(oldDeveloper) && string.IsNullOrEmpty(newDeveloper);
            var reAssignment = (!string.IsNullOrEmpty(oldDeveloper) && !string.IsNullOrEmpty(newDeveloper)) && (oldDeveloper != newDeveloper);

            //Case 1: oldDeveloper = null and newDeveloper = null
            //do nothing
            //Case 2: oldDeveloper has a value and newDeveloper has the same value
            //do nothing
            //Case 3: oldDeveloper = null and newDeveloper has a value
            //a new assignment has occurred, so newDeveloper must be informed
            //case 4: oldDeveloper has a value and newDeveloper = null
            //an unassignment. oldDeveloper must be notified
            //Case 5: oldDeveloper has a value and newDeveloper has a different value
            //a reassignment. both devs must be notified.

            if (newAssignment)
            {
                AddAssignmentNotification(ticketId, newDeveloper);
            }
            else if (unAssignment)
            {
                AddUnassignmentNotification(ticketId, oldDeveloper);
            }
            else if (reAssignment)
            {
                AddAssignmentNotification(ticketId, newDeveloper);
                AddUnassignmentNotification(ticketId, oldDeveloper);
            }
        }

        private void AddAssignmentNotification(int ticketId, string newDeveloper)
        {
            var properTicketId = db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            var newNotification = new TicketNotification
            {
                Created = DateTimeOffset.UtcNow.ToLocalTime(),
                TicketId = ticketId,
                Unread = true,
                UserId = newDeveloper,
                NotificationBody = $"You have been assigned to ticket '{properTicketId.Title}'."
            };
            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();
        }

        private void AddUnassignmentNotification(int ticketId, string oldDeveloper)
        {
            var properTicketId = db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            var oldNotification = new TicketNotification
            {
                Created = DateTimeOffset.UtcNow.ToLocalTime(),
                TicketId = ticketId,
                Unread = true,
                UserId = oldDeveloper,
                NotificationBody = $"You have been unassigned from ticket '{properTicketId.Title}'."
            };
            db.TicketNotifications.Add(oldNotification);
            db.SaveChanges();
        }

        public ICollection<TicketNotification> ListUserNotifications(string userId)
        {
            var unreadNotifications = db.TicketNotifications.Where(t => t.UserId == userId).ToList();
            return (unreadNotifications);
        }

        public ICollection<TicketNotification> ListUserUnreadNotifications(string userId)
        {
            var unreadNotifications = db.TicketNotifications.Where(t => t.Unread && t.UserId == userId).ToList();
            return (unreadNotifications);
        }
    }
}