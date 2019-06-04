using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.Models
{
    public class ProjectNotification
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public string NotificationBody { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool Unread { get; set; }

        //parent
        public virtual Project Project { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}