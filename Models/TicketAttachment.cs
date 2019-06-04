using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TicketID { get; set; }
        public string FilePath { get; set; }
        [StringLength(200, MinimumLength = 4, ErrorMessage = "The Attachment Description must be between 4 and 50 characters long.")]
        public string AttachmentDescription { get; set; }
        public DateTimeOffset Created { get; set; }

        //parents
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}