using CompSpyWeb.DAL;
using CompSpyWeb.Models;
using CompSpyWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CompSpyWeb.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("", "Home");
            }
            if (CheckUserPermission())
            {
                var ctx = new CompSpyContext();
                return View(ctx.Users.ToList());
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
                using (var ctx = new CompSpyContext())
                {
                    user.CreatedOn = DateTime.Now;
                    user.CreatorID = (int)Session["UserID"];
                    user.Password = HashPassword(user.Password);
                    ctx.Users.Add(user);
                    ctx.SaveChanges();
                }
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
                using (var ctx = new CompSpyContext())
                {
                    var employee = (from user in ctx.Users
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
                using (var ctx = new CompSpyContext())
                {
                    var userToEdit = (from u in ctx.Users
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
                    ctx.SaveChanges();
                }
                return RedirectToAction("", "User");
            }
            return RedirectToAction("", "Home");
        }

        private bool CheckUserPermission()
        {
            int uid = (int)Session["UserID"];
            using (var ctx = new CompSpyContext())
            {
                var us = ctx.Users.Where(u => u.UserID == uid).FirstOrDefault();
                return us.IsAdmin;
            }
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
    }
}