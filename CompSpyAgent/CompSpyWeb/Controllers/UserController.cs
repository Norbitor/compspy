using CompSpyWeb.DAL;
using CompSpyWeb.Models;
using CompSpyWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CompSpyWeb.Controllers
{
    public class UserController : Controller
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
        public ActionResult New([Bind(Include = "Login, FirstName, LastName, IsAdmin, Password")]User user)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                user.CreatedOn = DateTime.Now;
                user.CreatorID = (int)Session["UserID"];
                user.Password = HashPassword(user.Password);
                db.Users.Add(user);
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
                var employee = (from user in db.Users
                                where user.UserID == id
                                select new EditUserViewModel()
                                {
                                    UserID = user.UserID,
                                    Login = user.Login,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    IsAdmin = user.IsAdmin
                                }).FirstOrDefault();
                return View(employee);
            }
            return RedirectToAction("", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel user)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                var userToEdit = (from u in db.Users
                                    where u.UserID == user.UserID
                                    select u).FirstOrDefault();
                userToEdit.Login = user.Login;
                userToEdit.FirstName = user.FirstName;
                userToEdit.LastName = user.LastName;
                userToEdit.IsAdmin = user.IsAdmin;

                if (user.Password != null)
                {
                    if (user.Password == user.ConfirmPassword)
                    {
                        userToEdit.Password = HashPassword(user.Password);
                    } else
                    {
                        user.Password = user.ConfirmPassword = "";
                        ModelState.AddModelError("", "Podane hasła nie zgadzają się");
                        return View(user);
                    }
                }

                userToEdit.LastEdit = DateTime.Now;
                userToEdit.EditorID = (int)Session["UserID"];
                db.SaveChanges();
                return RedirectToAction("", "User");
            }
            return RedirectToAction("", "Home");
        }

        [HttpGet]
        public ActionResult Permissions(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (!CheckUserPermission())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Nie masz uprawnień do tego zasobu.");
            }

            var data = (from u in db.Users
                       where u.UserID == id
                       join p in db.ClassroomPermissions on u.UserID equals p.UserID into perms
                       from perm in perms.DefaultIfEmpty()
                       join c in db.Classrooms on perm.ClassroomID equals c.ID into cr
                       select new UserPermissionsViewModel()
                       {
                           UserID = u.UserID,
                           Login = u.Login,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           ClassroomsWithPermissions = cr.ToList()
                       }).FirstOrDefault();
            if(data == null)
            {
                return HttpNotFound("Uzytkownik o podanym ID nie zostal odnaleziony.");
            }

            data.AllClassrooms = db.Classrooms.ToList();
            return View(data);
        }

        private bool CheckUserPermission()
        {
            int uid = (int)Session["UserID"];
            var us = db.Users.Where(u => u.UserID == uid).FirstOrDefault();
            return us.IsAdmin;
        }

        private string HashPassword(string plainPassword)
        {
            byte[] pass = Encoding.Default.GetBytes(plainPassword);
            using (var sha256 = SHA256.Create())
            {
                byte[] hashPass = sha256.ComputeHash(pass);
                return BitConverter.ToString(hashPass).Replace("-", string.Empty);
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