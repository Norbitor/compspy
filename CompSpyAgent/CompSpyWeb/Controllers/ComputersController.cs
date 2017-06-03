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
    public class ComputersController : Controller
    {
        private CompSpyContext db = new CompSpyContext();

        // GET: Computers
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                var computers = db.Computers.Include(c => c.Classroom).Include(c => c.Creator).Include(c => c.Editor);
                return View(computers.ToList());
            }
            return RedirectToAction("", "Home");
            
        }


        // GET: Computers/Create
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                ViewBag.ClassroomID = new SelectList(db.Classrooms, "ClassroomID", "Name");
                return View();
            }
            return RedirectToAction("", "Home");
            
        }

        // POST: Computers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ClassroomID,IPAddress,StationDiscriminant")] Computer computer)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                if (ModelState.IsValid)
                {
                    computer.CreatorID = (int)Session["UserID"];
                    computer.CreatedOn = DateTime.Now;
                    db.Computers.Add(computer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ClassroomID = new SelectList(db.Classrooms, "ClassroomID", "Name", computer.ClassroomID);
                return View(computer);
            }
            return RedirectToAction("", "Home");

            
        }

        // GET: Computers/Edit/5
        [HttpGet]
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
                Computer computer = db.Computers.Find(id);
                if (computer == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ClassroomID = new SelectList(db.Classrooms, "ClassroomID", "Name", computer.ClassroomID);
                return View(computer);
            }
            return RedirectToAction("", "Home");

            
        }

        // POST: Computers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ClassroomID,IPAddress,StationDiscriminant")] Computer computer)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                var computerToEdit = db.Computers.Find(computer.ComputerID);
                if (ModelState.IsValid)
                {
                    computerToEdit.ClassroomID = computer.ClassroomID;
                    computerToEdit.IPAddress = computer.IPAddress;
                    computerToEdit.StationDiscriminant = computer.StationDiscriminant;
                    computerToEdit.EditorID = (int)Session["UserID"];
                    computerToEdit.lastEdit = DateTime.Now;
                    db.Entry(computerToEdit).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ClassroomID = new SelectList(db.Classrooms, "ClassroomID", "Name", computerToEdit.ClassroomID);
                return View(computerToEdit);
            }
            return RedirectToAction("", "Home");

            
        }

        // POST: Computers/Delete/5
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
                Computer computer = db.Computers.Find(id);
                db.Computers.Remove(computer);
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
