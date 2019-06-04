﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTimeOffset ChangedDate { get; set; }

        //parents
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}