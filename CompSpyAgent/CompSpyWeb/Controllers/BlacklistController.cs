using CompSpyWeb.DAL;
using CompSpyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompSpyWeb.Controllers
{
    public class BlacklistController : Controller
    {
        private CompSpyContext db = new CompSpyContext();

        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                return View(db.Blacklists.ToList());
            }
            return RedirectToAction("", "Home");
        }

        [HttpGet]
        public ActionResult New()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "ProcessName, ClassroomID")]Blacklist blacklist)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                blacklist.CreatedOn = DateTime.Now;
                blacklist.CreatorID = (int)Session["UserID"];
                db.Blacklists.Add(blacklist);
                db.SaveChanges();
                return RedirectToAction("", "Blacklist");
            }
            return RedirectToAction("", "Home");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                var blacklist = (from bl in db.Blacklists
                                 where bl.BlacklistID == id
                                 select bl).FirstOrDefault();
                ViewBag.ClassroomID = new SelectList(db.Classrooms, "ClassroomID", "Name", blacklist.ClassroomID);
                return View(blacklist);
            }
            return RedirectToAction("", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProcessName, ClassroomID")]Blacklist blacklist)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                var blacklistToEdit = (from p in db.Blacklists
                                       where p.BlacklistID == blacklist.BlacklistID
                                       select p).FirstOrDefault();
                blacklistToEdit.ProcessName = blacklist.ProcessName;
                blacklistToEdit.ClassroomID = blacklist.ClassroomID;
                blacklistToEdit.LastEdit = DateTime.Now;
                blacklistToEdit.EditorID = (int)Session["UserID"];

                db.SaveChanges();
                return RedirectToAction("", "Blacklist");
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