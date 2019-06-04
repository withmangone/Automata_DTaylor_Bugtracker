using Automata_DTaylor_Bugtracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.Helpers
{
    public class TicketHistoryHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public void RecordTicketChanges(Ticket oldTicket, Ticket newTicket)
        {
            //compare the old ticket property values to the new ticket. if different, we add a new ticket history record



            if (oldTicket.Title != newTicket.Title)
            {
                //record a ticket history for the title property
                AddTicketHistory(newTicket.Id, "Title", oldTicket.Title, newTicket.Title);
            }
            if (oldTicket.Description != newTicket.Description)
            {
                //record a ticket history for the title property
                AddTicketHistory(newTicket.Id, "Description", oldTicket.Description, newTicket.Description);
            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                //record a ticket history for the title property
                AddTicketHistory(newTicket.Id, "TicketStatusId", oldTicket.TicketStatusId.ToString(), newTicket.TicketStatusId.ToString());
            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                //record a ticket history for the title property
                AddTicketHistory(newTicket.Id, "TicketPriorityId", oldTicket.TicketPriorityId.ToString(), newTicket.TicketPriorityId.ToString());
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                //record a ticket history for the title property
                AddTicketHistory(newTicket.Id, "TicketTypeId", oldTicket.TicketTypeId.ToString(), newTicket.TicketTypeId.ToString());
            }
            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                //record a ticket history for the title property
                AddTicketHistory(newTicket.Id, "AssignedToUserId", oldTicket.AssignedToUserId, newTicket.AssignedToUserId);
            }
        }

        public void AddTicketHistory(int ticketId, string propertyName, string oldValue, string newValue)
        {
            var newTicketHistory = new TicketHistory
            {
                Property = propertyName,
                OldValue = oldValue,
                NewValue = newValue,
                ChangedDate = DateTimeOffset.UtcNow.ToLocalTime(),
                UserId = HttpContext.Current.User.Identity.GetUserId(),
                TicketId = ticketId
            };

            db.TicketHistories.Add(newTicketHistory);
            db.SaveChanges();
        }

        public static string GetHistoryDataFromId(string id, string property)
        {
            if (string.IsNullOrEmpty(id))
            {
                return "";
            }
            var data = id;
            switch(property)
            {
                case "AssignedToUserId":
                    data = db.Users.Find(id).FullName;
                    break;
                case "TicketStatusId":
                    data = db.TicketStatuses.Find(Convert.ToInt32(id)).Name;
                    break;
                case "TicketPriorityId":
                    data = db.TicketPriorities.Find(Convert.ToInt32(id)).Name;
                    break;
                case "TicketTypeId":
                    data = db.TicketTypes.Find(Convert.ToInt32(id)).Name;
                    break;
                default:
                    break;
            }
            return data;
        }

        public static string GetPropertyName(string property)
        {
            var data = property;
            switch (property)
            {
                case "AssignedToUserId":
                    data = "Assigned Developer";
                    break;
                case "TicketStatusId":
                    data = "Ticket Status";
                    break;
                case "TicketPriorityId":
                    data = "Ticket Priority";
                    break;
                case "TicketTypeId":
                    data = "Ticket Type";
                    break;
                default:
                    break;
            }
            return data;
        }
    }
}