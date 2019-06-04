using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Automata_DTaylor_Bugtracker.Models;
using Microsoft.AspNet.Identity;

namespace Automata_DTaylor_Bugtracker.Controllers
{
    public class ToDoListItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void MarkAsDone(int id)
        {
            var myItem = db.ToDoListItems.Find(id);
            myItem.IsChecked = !myItem.IsChecked;
            db.SaveChanges();
        }

        // GET: ToDoListItems
        //public ActionResult Index()
        //{
        //    var toDoListItems = db.ToDoListItems.Include(t => t.User);
        //    return View(toDoListItems.ToList());
        //}

        // GET: ToDoListItems/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ToDoListItem toDoListItem = db.ToDoListItems.Find(id);
        //    if (toDoListItem == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(toDoListItem);
        //}

        // GET: ToDoListItems/Create
        //public ActionResult Create()
        //{
        //    ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "FirstName");
        //    return View();
        //}

        // POST: ToDoListItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Name,IsChecked")] ToDoListItem toDoListItem)
        {
            if (ModelState.IsValid)
            {
                toDoListItem.UserId = User.Identity.GetUserId();
                db.ToDoListItems.Add(toDoListItem);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            //ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", toDoListItem.UserId);
            return View(toDoListItem);
        }

        // GET: ToDoListItems/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ToDoListItem toDoListItem = db.ToDoListItems.Find(id);
        //    if (toDoListItem == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", toDoListItem.UserId);
        //    return View(toDoListItem);
        //}

        // POST: ToDoListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,UserId,Name,IsChecked")] ToDoListItem toDoListItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(toDoListItem).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", toDoListItem.UserId);
        //    return View(toDoListItem);
        //}

        // GET: ToDoListItems/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ToDoListItem toDoListItem = db.ToDoListItems.Find(id);
        //    if (toDoListItem == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(toDoListItem);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomDelete()
        {
            var userId = User.Identity.GetUserId();
            foreach(var item in db.ToDoListItems.Where(i => i.UserId == userId && i.IsChecked))
            {
                db.ToDoListItems.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // POST: ToDoListItems/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ToDoListItem toDoListItem = db.ToDoListItems.Find(id);
        //    db.ToDoListItems.Remove(toDoListItem);
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
