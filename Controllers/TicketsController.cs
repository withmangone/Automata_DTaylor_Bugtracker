using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Automata_DTaylor_Bugtracker.Helpers;
using Automata_DTaylor_Bugtracker.Models;
using Microsoft.AspNet.Identity;

namespace Automata_DTaylor_Bugtracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projectHelper = new ProjectHelper();
        private RoleHelper roleHelper = new RoleHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private TicketHistoryHelper historyHelper = new TicketHistoryHelper();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            db.Tickets.Where(t => !t.Deleted).OrderBy(t => t.TicketPriority.Importance).ToList();
            return View(projectHelper.ListUserTickets(User.Identity.GetUserId()));
        }

        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var authorized = false;
            switch (userRole)
            {
                case "Submitter":
                    authorized = ticket.OwnerUserId == userId;
                    break;
                case "Developer":
                    authorized = ticket.AssignedToUserId == userId;
                    break;
                case "Admin":
                    authorized = true;
                    break;
                case "ProjectManager":
                    var projectIds = db.Users.Find(userId).Projects.Select(p => p.Id).ToList();
                    authorized = projectIds.Contains(ticket.ProjectId);
                    break;
            }
            if (authorized == true)
            {
                return View(ticket);
            }
            else
            {
                return RedirectToAction("Permissions", "Admin");
            }
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(int? projectId)
        {
            if(User.IsInRole("Submitter"))
            {
                var myTicket = new Ticket();
                if (projectId == null)
                {
                    var userId = User.Identity.GetUserId();
                    var myProjects = projectHelper.ListUserProjects(userId);
                    ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");
                    myTicket.ProjectId = -1;
                }
                else
                {
                    myTicket.ProjectId = (int)projectId;
                }
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
                return View(myTicket);
            }
            else
            {
                return RedirectToAction("Permissions", "Admin");
            }
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId,Title,Description,Created,Updated")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTimeOffset.UtcNow.ToLocalTime();
                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id;
                ticket.OwnerUserId = User.Identity.GetUserId();
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FullContactInfo", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FullContactInfo", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            var myProjects = projectHelper.ListUserProjects(userId);
            var developers = new List<ApplicationUser>();
            var usersOnProject = projectHelper.UsersOnProject(ticket.ProjectId);
            foreach (var user in usersOnProject)
            {
                if (roleHelper.IsUserInRole(user.Id, "Developer"))
                {
                    developers.Add(user);
                }
            }

            ViewBag.AssignedToUserId = new SelectList(developers, "Id", "FullContactInfo", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FullContactInfo", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);

            //authorization
            var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var authorized = false;
            switch (userRole)
            {
                case "Submitter":
                    authorized = ticket.OwnerUserId == userId;
                    break;
                case "Developer":
                    authorized = ticket.AssignedToUserId == userId;
                    break;
                case "Admin":
                    authorized = true;
                    break;
                case "ProjectManager":
                    var projectIds = db.Users.Find(userId).Projects.Select(p => p.Id).ToList();
                    authorized = projectIds.Contains(ticket.ProjectId);
                    break;
            }
            if (authorized == true)
            {
                return View(ticket);
            }
            else
            {
                return RedirectToAction("Permissions", "Admin");
            }
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId,Title,Description,Created,Updated")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //there are times when i need to compare the new value of a ticket property against the old val. when i have to do this i need to go out to the db and grab the ticket before the db.savechanges in order to have a copy of the old ticket. now i can compare the old ticket to the new incoming ticket
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                //an assignment has occurred.
                //TriggerNotifications()
                notificationHelper.TriggerAssignmentNotifications(ticket.Id, oldTicket.AssignedToUserId, ticket.AssignedToUserId);
                //UpdateTicketStatus()
                //what if i manually changed the status on the form? if so, i dont want to overrule
                if (oldTicket.TicketStatusId == ticket.TicketStatusId)
                {
                    ticket.TicketStatusId = ticketHelper.GetNewTicketStatus(oldTicket.AssignedToUserId, ticket.AssignedToUserId);
                }

                //call our ticket history helper to record any meaningful changes in property values
                historyHelper.RecordTicketChanges(oldTicket, ticket);

                ticket.Updated = DateTimeOffset.UtcNow.ToLocalTime();
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Archive(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            ticket.Deleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
