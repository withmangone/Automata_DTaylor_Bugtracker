using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Automata_DTaylor_Bugtracker.Helpers;
using Automata_DTaylor_Bugtracker.Models;
using Microsoft.AspNet.Identity;

namespace Automata_DTaylor_Bugtracker.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        //public ActionResult Index()
        //{
        //    var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
        //    return View(ticketAttachments.ToList());
        //}

        // GET: TicketAttachments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
        //    if (ticketAttachment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticketAttachment);
        //}

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,TicketID,FilePath,AttachmentDescription")] TicketAttachment ticketAttachment, HttpPostedFileBase attachment)
        {
            if (ModelState.IsValid)
            {
                if (attachment != null)
                {
                    //start processing my image
                    if (AttachmentHelper.IsWebFriendlyAttachment(attachment))
                    {
                        var fileName = Path.GetFileName(attachment.FileName).Replace(' ', '_');
                        attachment.SaveAs(Path.Combine(Server.MapPath("~/Attachments/"), fileName));
                        ticketAttachment.FilePath = "/Attachments/" + fileName;
                    }
                }

                ticketAttachment.Created = DateTimeOffset.UtcNow.ToLocalTime();
                ticketAttachment.UserId = User.Identity.GetUserId();
                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketID});
            }

            ViewBag.TicketID = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketAttachment.TicketID);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
        //    if (ticketAttachment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.TicketID = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketAttachment.TicketID);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
        //    return View(ticketAttachment);
        //}

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,TicketID,FilePath,Description,Created")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketID = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketAttachment.TicketID);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
        //    if (ticketAttachment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticketAttachment);
        //}

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
