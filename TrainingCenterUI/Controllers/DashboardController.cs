using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingCenterUI.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            if (Session["login"] != null) 
            {
                return View();
            }
            else 
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        public ActionResult Instructor()
        {

            if (Session["login"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}