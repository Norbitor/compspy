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

                //int uid = (int)Session["UserID"];
                //classrooms = (from c in db.Classrooms
                //             join cp in db.ClassroomPermissions on c.ID equals cp.ClassroomID
                //             where cp.UserID == uid
                //             select new ClassroomViewModel()
                //             {
                //                 Name = c.Name,
                //                 Location = c.Location
                //             }).ToList();
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