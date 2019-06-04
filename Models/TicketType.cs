﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //children
        public virtual ICollection<Ticket> Tickets { get; set; }

        public TicketType()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}