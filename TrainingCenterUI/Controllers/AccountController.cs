using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static TrainingCenterUI.Models.AccountModel;
using System.Web.Security;
using TrainingCenterSecurity.Entities;
using WebMatrix.WebData;
using WebMatrix.Data;
using System.Data.SqlClient;
using TrainingCenterLib.Repository;

namespace TrainingCenterUI.Controllers
{
    public class AccountController : Controller
    {

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("MembershipDbContext", "Users", "UserId", "UserName", autoCreateTables: true);
            }
            LoginModel loginModel = new LoginModel();
            return View(loginModel);
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: false))
            {
                using (var db = new TrainingCenterSecurityDbContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.UserName == model.UserName);
                    if (user != null)
                    {
                        //WaslnyLibRepository.currentuserId = user.UserId;
                    }
                }
                Session["login"] = true;
                //UserInfo.GlobalUserID = WebSecurity.CurrentUserId;
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Session["login"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("MembershipDbContext", "Users", "UserId", "UserName", autoCreateTables: true);
            }
            RegisterModel registerModel = new RegisterModel();
            return View(registerModel);
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            
            if (ModelState.IsValid)
            {
                using (var db = new TrainingCenterSecurityDbContext())
                {
                    if (db.Users.Any(u => u.UserName == model.UserName))
                    {
                        ModelState.AddModelError("", "UserName already exists. Please choose a different UserName.");
                        return View(model);
                    }
                    try
                    {
                        WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                        return RedirectToAction("Index", "Home");
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ///.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                }
            }

            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            Session["login"] = null;
            return RedirectToAction("Login", "Account");
        }


    }
}