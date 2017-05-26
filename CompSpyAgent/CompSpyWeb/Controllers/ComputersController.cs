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
            var computers = db.Computers.Include(c => c.Classroom).Include(c => c.Creator).Include(c => c.Editor);
            return View(computers.ToList());
        }

        // GET: Computers/Create
        public ActionResult Create()
        {
            ViewBag.ClassroomID = new SelectList(db.Classrooms, "ID", "Name");
            return View();
        }

        // POST: Computers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ClassroomID,IPAddress,StationDiscriminant")] Computer computer)
        {
            if (ModelState.IsValid)
            {
                computer.CreatorID = (int)Session["UserID"];
                computer.CreatedOn = DateTime.Now;
                db.Computers.Add(computer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassroomID = new SelectList(db.Classrooms, "ID", "Name", computer.ClassroomID);
            return View(computer);
        }

        // GET: Computers/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ClassroomID = new SelectList(db.Classrooms, "ID", "Name", computer.ClassroomID);
            return View(computer);
        }

        // POST: Computers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ClassroomID,IPAddress,StationDiscriminant")] Computer computer)
        {
            var computerToEdit = db.Computers.Find(computer.ID);
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
            ViewBag.ClassroomID = new SelectList(db.Classrooms, "ID", "Name", computerToEdit.ClassroomID);
            return View(computerToEdit);
        }

        // POST: Computers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Computer computer = db.Computers.Find(id);
            db.Computers.Remove(computer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Connect(string stationId, string secret)
        {
            var comp = (from c in db.Computers
                       where c.StationDiscriminant == stationId
                       select c).FirstOrDefault();

            if (comp != null)
            {
                comp.IsConnected = true;
                db.Entry(comp).State = EntityState.Modified;
                db.SaveChanges();
                return Content("SUCCESS");
            } else
            {
                return Content("FAIL Computer is not authorized!");
            }
        }

        [HttpPost]
        public ActionResult Disconnect(string stationId, string secret)
        {
            var comp = (from c in db.Computers
                       where c.StationDiscriminant == stationId
                       select c).FirstOrDefault();

            if (comp != null)
            {
                comp.IsConnected = false;
                db.Entry(comp).State = EntityState.Modified;
                db.SaveChanges();
                return Content("SUCCESS");
            }
            else
            {
                return Content("FAIL Computer is not authorized!");
            }
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
