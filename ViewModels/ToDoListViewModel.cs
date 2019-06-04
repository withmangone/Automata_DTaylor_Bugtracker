using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.ViewModels
{
    public class ToDoListViewModel
    {
        public List<ToDoListItemViewModel> MyList { get; set; }
    }

    public class ToDoListItemViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}