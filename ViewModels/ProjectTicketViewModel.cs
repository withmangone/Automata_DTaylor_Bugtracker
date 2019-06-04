using Automata_DTaylor_Bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.ViewModels
{
    public class ProjectTicketViewModel
    {
        public Ticket Ticket { get; set; }
        public Project Project { get; set; }
    }
}