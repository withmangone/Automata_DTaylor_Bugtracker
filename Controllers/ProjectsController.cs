using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Automata_DTaylor_Bugtracker.Models;
using Automata_DTaylor_Bugtracker.ViewModels;
using Automata_DTaylor_Bugtracker.Helpers;
using Microsoft.AspNet.Identity;

namespace Automata_DTaylor_Bugtracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private ProjectNotificationHelper projectNotificationHelper = new ProjectNotificationHelper();

        // GET: Projects
        [Authorize]
        public ActionResult MyIndex()
        {
            return View(projectHelper.ListUserProjects(User.Identity.GetUserId()).Where(p => !p.Deleted));
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Index()
        {
            
            return View(db.Projects.Where(p => !p.Deleted).ToList());
        }

        // GET: Projects/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            
            //authorization
            var userId = User.Identity.GetUserId();
            var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var authorized = false;
            var projectIds = db.Users.Find(userId).Projects.Select(p => p.Id).ToList();
            switch (userRole)
            {
                case "Submitter":
                    authorized = projectIds.Contains(project.Id);
                    break;
                case "Developer":
                    authorized = projectIds.Contains(project.Id);
                    break;
                case "Admin":
                    authorized = true;
                    break;
                case "ProjectManager":
                    authorized = true;
                    break;
            }
            if (authorized == true)
            {
                return View(project);
            }
            else
            {
                return RedirectToAction("Permissions", "Admin");
            }
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);

            //what users occupy the role of developer
            var developers = roleHelper.UsersInRole("Developer");
            var submitters = roleHelper.UsersInRole("Submitter");
            var pM = roleHelper.UsersInRole("ProjectManager");

            //once i associate people to the project i will want to see who is on the project. in order to do this i will use the 4th parameter
            var devsOnProject = new List<string>();
            var subsOnProject = new List<string>();
            var pmOnProject = "";
            foreach (var user in projectHelper.UsersOnProject(project.Id))
            {
                var userRole = roleHelper.ListUserRoles(user.Id).FirstOrDefault();
                switch (userRole)
                {
                    case "Developer":
                        devsOnProject.Add(user.Id);
                        break;
                    case "Submitter":
                        subsOnProject.Add(user.Id);
                        break;
                    case "ProjectManager":
                        pmOnProject = user.Id;
                        break;
                    default:
                        break;
                }
            }

            //i need viewbag properties to hold Devs, Subs and PMs
            ViewBag.Developers = new MultiSelectList(developers, "Id", "FullContactInfo", devsOnProject);
            ViewBag.Submitters = new MultiSelectList(submitters, "Id", "FullContactInfo", subsOnProject);
            ViewBag.PM = new SelectList(pM, "Id", "FullContactInfo", pmOnProject);

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Project project, List<string> Developers, List<string> Submitters, string PM)
        {
            if (ModelState.IsValid)
            {
                var oldUsersOnProject = projectHelper.UsersOnProject(project.Id).ToList();

                //unassign everyone from the project. must be ToList so the foreach loop doesnt constantly modify itself over an over
                foreach (var user in projectHelper.UsersOnProject(project.Id).ToList())
                {
                    projectHelper.RemoveUserFromProject(user.Id, project.Id);
                }

                //add back to the project all the selected developers
                if (Developers != null)
                {
                    foreach (var userId in Developers)
                    {
                        projectHelper.AddUserToProject(userId, project.Id);
                    }
                }

                //add back to the project all the selected submitters
                if (Submitters != null)
                {
                    foreach (var userId in Submitters)
                    {
                        projectHelper.AddUserToProject(userId, project.Id);
                    }
                }

                //add back to the project the selected PM
                if (PM != null)
                {
                    projectHelper.AddUserToProject(PM, project.Id);
                }

                projectNotificationHelper.TriggerProjectAssignmentNotifications(oldUsersOnProject, project.Id);

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Archive(int id)
        {
            Project project = db.Projects.Find(id);
            project.Deleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
