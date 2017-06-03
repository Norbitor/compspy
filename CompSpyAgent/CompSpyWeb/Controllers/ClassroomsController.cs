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
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                var classrooms = db.Classrooms.Include(c => c.Creator).Include(c => c.Editor);
                return View(classrooms.ToList());
            }
            return RedirectToAction("", "Home");
            
        }

        // GET: Classrooms/Create
        public ActionResult Create()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                return View();
            }
            return RedirectToAction("", "Home");
            
        }

        // POST: Classrooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Location")] Classroom classroom)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
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
            return RedirectToAction("", "Home");

            
        }

        // GET: Classrooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
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
            return RedirectToAction("", "Home");

            
        }

        // POST: Classrooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Location")] Classroom classroom)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                var classroomToEdit = db.Classrooms.Find(classroom.ClassroomID);
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
            return RedirectToAction("", "Home");

        }

        // POST: Classrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                Classroom classroom = db.Classrooms.Find(id);
                db.Classrooms.Remove(classroom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("", "Home");
        }

        private bool CheckUserPermission()
        {
            int uid = (int)Session["UserID"];
            var us = db.Users.Where(u => u.UserID == uid).FirstOrDefault();
            return us.IsAdmin;
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
