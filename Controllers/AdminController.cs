using Automata_DTaylor_Bugtracker.Helpers;
using Automata_DTaylor_Bugtracker.Models;
using Automata_DTaylor_Bugtracker.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Automata_DTaylor_Bugtracker.Controllers
{
    public class AdminController : Controller
    {
        //i want to add a class level property that can access the db
        private ApplicationDbContext db = new ApplicationDbContext();

        //i want to add a class level property that can help manage roles
        private RoleHelper roleHelper = new RoleHelper();

        //i want to add a class level property that can help manage projects and the people on them
        private ProjectHelper projHelper = new ProjectHelper();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserRoles(string users, string roles)
        {
            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "FullContactInfo");
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name");
            //i need to remove the user from all projects and tickets they've been assigned to, since their responsibilities have changed
            var userId = db.Users.Find(users).Id;

            if (roleHelper.IsUserInRole(userId, "Developer") == true)
            {
                var myTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                foreach (var ticket in myTickets)
                {
                    ticket.AssignedToUserId = null;
                    var unassigned = db.TicketStatuses.FirstOrDefault(s => s.Name == "Unassigned");
                    ticket.TicketStatusId = unassigned.Id;
                }
            }
            db.SaveChanges();

            //i want to ensure that the person i selected occupies only one role. therefore the first thing ill do is remove the user from a current role.
            foreach (var role in roleHelper.ListUserRoles(users))
            {
                roleHelper.RemoveUserFromRole(users, role);
            }
            roleHelper.AddUserToRole(users, roles);

            return RedirectToAction("UserIndex", "Admin");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UserIndex()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "FullContactInfo");
                ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name");
                var users = db.Users.Select(u => new UserViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                }).ToList();
                return View(users);
            }
            else
            {
                return RedirectToAction("Permissions", "Admin");
            }
        }

        public ActionResult Permissions()
        {
            return View();
        }
    }
}