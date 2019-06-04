using Automata_DTaylor_Bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.ViewModels
{
    public class CollectedNotifications
    {
        public List<ProjectNotification> ProjectNotifications { get; set; }
        public List<TicketNotification> TicketNotifications { get; set; }

        public CollectedNotifications()
        {
            ProjectNotifications = new List<ProjectNotification> ();
            TicketNotifications = new List<TicketNotification>();
        }
    }
}