using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 4, ErrorMessage = "The Name must be between 4 and 50 characters long.")]
        public string Name { get; set; }
        [StringLength(250, MinimumLength = 4, ErrorMessage = "The Description must be between 4 and 250 characters long.")]
        public string Description { get; set; }
        public bool Deleted { get; set; }

        //children
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<ProjectNotification> ProjectNotifications { get; set; }

        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
            ProjectNotifications = new HashSet<ProjectNotification>();
        }
    }
}