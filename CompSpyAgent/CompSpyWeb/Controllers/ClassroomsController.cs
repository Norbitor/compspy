using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CompSpyWeb.DAL;
using CompSpyWeb.Models;

namespace CompSpyWeb.Controllers
{
    public class ClassroomsController : Controller
    {
        private CompSpyContext db = new CompSpyContext();

        // GET: Classrooms
        public ActionResult Index()
        {
            var classrooms = db.Classrooms.Include(c => c.Creator).Include(c => c.Editor);
            return View(classrooms.ToList());
        }

        // GET: Classrooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Classrooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Location")] Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                classroom.CreatorID = (int)Session["UserID"];
                classroom.CreatedOn = DateTime.Now;
                db.Classrooms.Add(classroom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(classroom);
        }

        // GET: Classrooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        // POST: Classrooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Location")] Classroom classroom)
        {
            var classroomToEdit = db.Classrooms.Find(classroom.ID);
            if (ModelState.IsValid && classroomToEdit != null)
            {
                classroomToEdit.Name = classroom.Name;
                classroomToEdit.Location = classroom.Location;
                classroomToEdit.EditorID = (int)Session["UserID"];
                classroomToEdit.LastEdit = DateTime.Now;
                db.Entry(classroomToEdit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(classroomToEdit);
        }

        // POST: Classrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Classroom classroom = db.Classrooms.Find(id);
            db.Classrooms.Remove(classroom);
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
