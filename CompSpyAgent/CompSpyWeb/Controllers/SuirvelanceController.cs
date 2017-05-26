using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompSpyWeb.Controllers
{
    public class SuirvelanceController : Controller
    {
        // GET: Suirvelance
        public ActionResult Index()
        {
            return View();
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
    }
}