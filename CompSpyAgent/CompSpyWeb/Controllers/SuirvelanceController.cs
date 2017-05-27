using CompSpyWeb.DAL;
using CompSpyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompSpyWeb.Controllers
{
    public class SuirvelanceController : Controller
    {
        private CompSpyContext db = new CompSpyContext();

        // GET: Suirvelance
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            ICollection<Classroom> classrooms;
            if (CheckUserPermission())
            {
                classrooms = db.Classrooms.ToList();
            }
            else
            {
                int uid = (int)Session["UserID"];
                classrooms = (from c in db.Classrooms
                             join cp in db.ClassroomPermissions on c.ID equals cp.ClassroomID
                             where cp.UserID == uid
                             select c).ToList();
            }

            return View(classrooms);
        }

        // GET: Suirvelance/ShowRoom/4
        public ActionResult ShowRoom(int id)
        {
            return View();
        }

        // GET: Suirvelance/ShowComputer/34
        public ActionResult ShowComputer(int id)
        {
            return View();
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