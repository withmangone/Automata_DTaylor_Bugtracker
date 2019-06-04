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
using Automata_DTaylor_Bugtracker.ViewModels;
using Microsoft.AspNet.Identity;

namespace Automata_DTaylor_Bugtracker.Controllers
{
    public class TicketNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private ProjectNotificationHelper projectNotificationHelper = new ProjectNotificationHelper();

        // GET: TicketNotifications
        [Authorize]
        public ActionResult Index()
        {
            //var ticketNotifications = notificationHelper.ListUserNotifications(User.Identity.GetUserId());
            var userId = User.Identity.GetUserId();
            var collectedNotifications = new CollectedNotifications
            {
                TicketNotifications = db.TicketNotifications.Where(t => t.UserId == userId).ToList(),
                ProjectNotifications = db.ProjectNotifications.Where(t => t.UserId == userId).ToList()
            };
            return View(collectedNotifications);
        }

        public ActionResult UnreadIndex()
        {
            var userId = User.Identity.GetUserId();
            var collectedNotifications = new CollectedNotifications
            {
                TicketNotifications = db.TicketNotifications.Where(t => t.UserId == userId && t.Unread).ToList(),
                ProjectNotifications = db.ProjectNotifications.Where(t => t.UserId == userId && t.Unread).ToList()
            };
            return View(collectedNotifications);
        }

        public ActionResult MarkAsRead(int? id)
        {
            var ticketNotification = db.TicketNotifications.Find(id);
            db.TicketNotifications.Attach(ticketNotification);
            ticketNotification.Unread = false;
            db.SaveChanges();

            return RedirectToAction("UnreadIndex");
        }

        public ActionResult MarkProjectAsRead(int? id)
        {
            var projectNotification = db.ProjectNotifications.Find(id);
            db.ProjectNotifications.Attach(projectNotification);
            projectNotification.Unread = false;
            db.SaveChanges();

            return RedirectToAction("UnreadIndex");
        }

        // GET: TicketNotifications/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketNotification ticketNotification = db.TicketNotifications.Find(id);
        //    if (ticketNotification == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticketNotification);
        //}

        // GET: TicketNotifications/Create
        //public ActionResult Create()
        //{
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId");
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
        //    return View();
        //}

        // POST: TicketNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,TicketId,UserId,NotificationBody")] TicketNotification ticketNotification)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.TicketNotifications.Add(ticketNotification);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketNotification.TicketId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotification.UserId);
        //    return View(ticketNotification);
        //}

        // GET: TicketNotifications/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketNotification ticketNotification = db.TicketNotifications.Find(id);
        //    if (ticketNotification == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketNotification.TicketId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotification.UserId);
        //    return View(ticketNotification);
        //}

        // POST: TicketNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,TicketId,UserId,NotificationBody")] TicketNotification ticketNotification)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(ticketNotification).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketNotification.TicketId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotification.UserId);
        //    return View(ticketNotification);
        //}

        // GET: TicketNotifications/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketNotification ticketNotification = db.TicketNotifications.Find(id);
        //    if (ticketNotification == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticketNotification);
        //}

        // POST: TicketNotifications/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    TicketNotification ticketNotification = db.TicketNotifications.Find(id);
        //    db.TicketNotifications.Remove(ticketNotification);
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
