using Automata_DTaylor_Bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.ViewModels
{
    public class UserProfileViewModel
    {
        public IndexViewModel IndexViewModel { get; set; }
        public UserViewModel UserViewModel { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
    }
}