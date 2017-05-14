using CompSpyWeb.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompSpyWeb.Controllers
{
    public class UserController : Controller
    {
        // GET: User
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

        private bool CheckUserPermission()
        {
            int uid = (int)Session["UserID"];
            using (var ctx = new CompSpyContext())
            {
                var us = ctx.Users.Where(u => u.UserID == uid).FirstOrDefault();
                return us.IsAdmin;
            }
        }
    }
}