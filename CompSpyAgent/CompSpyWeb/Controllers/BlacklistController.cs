using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompSpyWeb.Controllers
{
    public class BlacklistController
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
                return View(db.Users.ToList());
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
                return View();
            }
            return RedirectToAction("", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "ProcessName")]Blacklist blacklist)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                blacklist.CreatedOn = DateTime.Now;
                blacklist.CreatorID = (int)Session["UserID"];
                /*db.Users.Add(blacklist);*/
                db.SaveChanges();
                return RedirectToAction("", "User");
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
                /*
                var blacklist = (from user in db.Users
                                where user.UserID == id
                                select new EditUserViewModel()
                                {
                                    UserID = user.UserID,
                                    Login = user.Login,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    IsAdmin = user.IsAdmin
                                }).FirstOrDefault();
                return View(employee);*/
            }
            return RedirectToAction("", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blacklist process)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                /*var processToEdit = (from p in db.Users
                                  where p.UserID == process.UserID
                                  select p).FirstOrDefault();
                processToEdit.ProcessName = process.ProcessName;
                //userToEdit.FirstName = user.FirstName;
                //userToEdit.LastName = user.LastName;
                //userToEdit.IsAdmin = user.IsAdmin;
                
                processToEdit.LastEdit = DateTime.Now;
                processToEdit.EditorID = (int)Session["UserID"];
                db.SaveChanges();
                return RedirectToAction("", "Blacklist");*/
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