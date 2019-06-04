using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        [StringLength(250, MinimumLength = 4, ErrorMessage = "The Comment Body must be between 4 and 250 characters long.")]
        public string CommentBody { get; set; }
        public DateTimeOffset Created { get; set; }

        //parents
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}