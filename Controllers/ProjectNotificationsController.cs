using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Automata_DTaylor_Bugtracker.Models;

namespace Automata_DTaylor_Bugtracker.Controllers
{
    public class ProjectNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectNotifications
        //public ActionResult Index()
        //{
        //    var projectNotifications = db.ProjectNotifications.Include(p => p.Project).Include(p => p.User);
        //    return View(projectNotifications.ToList());
        //}

        // GET: ProjectNotifications/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProjectNotification projectNotification = db.ProjectNotifications.Find(id);
        //    if (projectNotification == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(projectNotification);
        //}

        // GET: ProjectNotifications/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
        //    return View();
        //}

        // POST: ProjectNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,UserId,NotificationBody,Created,Unread")] ProjectNotification projectNotification)
        {
            if (ModelState.IsValid)
            {
                db.ProjectNotifications.Add(projectNotification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectNotification.ProjectId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", projectNotification.UserId);
            return View(projectNotification);
        }

        // GET: ProjectNotifications/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProjectNotification projectNotification = db.ProjectNotifications.Find(id);
        //    if (projectNotification == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectNotification.ProjectId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", projectNotification.UserId);
        //    return View(projectNotification);
        //}

        // POST: ProjectNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,UserId,NotificationBody,Created,Unread")] ProjectNotification projectNotification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectNotification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectNotification.ProjectId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", projectNotification.UserId);
            return View(projectNotification);
        }

        // GET: ProjectNotifications/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProjectNotification projectNotification = db.ProjectNotifications.Find(id);
        //    if (projectNotification == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(projectNotification);
        //}

        // POST: ProjectNotifications/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ProjectNotification projectNotification = db.ProjectNotifications.Find(id);
        //    db.ProjectNotifications.Remove(projectNotification);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
