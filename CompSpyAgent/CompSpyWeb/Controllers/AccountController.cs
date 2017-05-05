using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity;
using CompSpyWeb.Models;
using CompSpyWeb.DAL;

namespace CompSpyWeb.Controllers
{
    public class AccountController : Controller
    {
        const int FAILED_LOGINS_LIMIT = 10;

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserID"] == null)
            {
                return View();
            }
            return RedirectToAction("", "Home");
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            using (var ctx = new CompSpyContext())
            {
                byte[] pass = Encoding.Default.GetBytes(password); //employee pass in bytes
                using (var sha256 = SHA256.Create())
                {
                    byte[] hashPass = sha256.ComputeHash(pass); //256-bits employee pass
                    string hashPassHex = BitConverter.ToString(hashPass).Replace("-", string.Empty); //64 chars hash pass

                    //get login and pass from DB
                    var user = ctx.Users.Where(e => e.Login == login).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.Password == hashPassHex) //user typed proper data
                        {
                            if (user.LoginAttempts < FAILED_LOGINS_LIMIT)
                            {
                                Session["UserID"] = user.UserID;
                                Session["Administrator"] = user.IsAdmin;
                                Session["Name"] = user.FirstName + " " + user.LastName;
                                user.LastLogin = DateTime.Now;
                                user.LoginAttempts = 0; // 0 the counter
                            }
                            else
                            {
                                return RedirectToAction("", "Home");
                            }
                        }
                        else //user typed incorrect password
                        {
                            if (user.LoginAttempts < FAILED_LOGINS_LIMIT)
                            {
                                user.LoginAttempts += 1;//add one because of failed login attempt
                            }
                            else
                            {
                                return RedirectToAction("", "Home");
                            }
                        }
                        ctx.Entry(user).State = EntityState.Modified;
                        ctx.SaveChanges();
                    }
                }

            }

            return RedirectToAction("", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logoff()
        {
            if (Session["UserID"] != null)
            {
                Session["UserID"] = null;
            }
            return RedirectToAction("", "Home");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string[] pass) //table of passwords
        {
            if (pass[0] != "" && pass[1] != "" && pass[2] != "")
            {
                if (Session["UserID"] != null)
                {
                    using (var ctx = new CompSpyContext())
                    {
                        int userID = (int)Session["UserID"];
                        var foundUser = ctx.Users.Where(x => x.UserID == userID).FirstOrDefault(); //employee

                        byte[] oldPassword = Encoding.Default.GetBytes(pass[0]); //employee old pass
                        using (var sha256 = SHA256.Create())
                        {
                            byte[] hashOldPass = sha256.ComputeHash(oldPassword); //256-bits employee pass
                            string hashOldPassHex = BitConverter.ToString(hashOldPass).Replace("-", string.Empty); //64 chars hash pass

                            if (hashOldPassHex == foundUser.Password) //user typed proper old pass
                            {
                                if (pass[1] == pass[2]) //user typed twice the same new pass
                                {
                                    byte[] newPass = Encoding.Default.GetBytes(pass[1]);
                                    byte[] hashNewPass = sha256.ComputeHash(newPass);
                                    string hashNewPassHex = BitConverter.ToString(hashNewPass).Replace("-", string.Empty);

                                    foundUser.Password = hashNewPassHex;
                                    foundUser.EditorID = (int)Session["UserID"];
                                    foundUser.LastEdit = DateTime.Now;
                                    ctx.Entry(foundUser).State = EntityState.Modified;
                                    ctx.SaveChanges();
                                    ViewData["Message"] = "OK";
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Podane hasła nie zgadzają się!");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Podane stare hasło jest nieprawidłowe!");
                            }

                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Przynajmniej jedno z wymaganych pól jest nieuzupełnione!");
            }
            return View();
        }
    }
}
