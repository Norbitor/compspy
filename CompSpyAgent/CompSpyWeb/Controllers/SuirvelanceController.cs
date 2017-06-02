using CompSpyWeb.DAL;
using CompSpyWeb.Models;
using CompSpyWeb.ViewModels;
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
            ICollection<ClassroomViewModel> classrooms;
            if (CheckUserPermission())
            {
                classrooms = PopulateClassroomList(null);
            }
            else
            {
                classrooms = PopulateClassroomList((int)Session["UserID"]);
            }

            return View(classrooms);
        }

        // GET: Suirvelance/ShowRoom/4
        public ActionResult ShowRoom(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            var classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound("Nie znaleziono sali o zadanym ID.");
            }
            var computers = db.Computers.Where(comp => comp.ClassroomID == classroom.ID).ToList();
            ViewBag.ClassroomName = classroom.Name;
            return View(computers);
        }

        // GET: Suirvelance/ShowComputer/34
        public ActionResult ShowComputer(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            var computer = db.Computers.Find(id);
            if (computer == null || !computer.IsConnected)
            {
                return HttpNotFound("Nie znaleziono komputera o zadanym ID lub komputer nie jest wlaczony.");
            }
            return View();
        }

        private ICollection<ClassroomViewModel> PopulateClassroomList(int? userid)
        {
            ICollection<ClassroomViewModel> classrooms;
            if (userid == null)
            {
                classrooms = (from classroom in db.Classrooms
                              join computer in db.Computers on classroom.ID equals computer.ClassroomID into computers
                              select new ClassroomViewModel()
                              {
                                  ID = classroom.ID,
                                  Name = classroom.Name,
                                  Location = classroom.Location,
                                  ComputersCount = computers.Count(),
                                  ActiveComputers = computers.Where(x => x.IsConnected).Count()
                              }).ToList();
            }
            else
            {
                classrooms = (from classroom in db.Classrooms
                              join cperm in db.ClassroomPermissions on classroom.ID equals cperm.ClassroomID
                              join computer in db.Computers on classroom.ID equals computer.ClassroomID into computers
                              where cperm.UserID == userid
                              select new ClassroomViewModel()
                              {
                                  ID = classroom.ID,
                                  Name = classroom.Name,
                                  Location = classroom.Location,
                                  ComputersCount = computers.Count(),
                                  ActiveComputers = computers.Where(x => x.IsConnected).Count()
                              }).ToList();
            }
            return classrooms;
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