using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SurveyMvc.Controllers
{
    public class AdminLoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserLoginModel user)
        {
            if (ModelState.IsValid)
            {
                int CustomerId = 0;
                if (user.IsValidUser(user.UserEmail, user.Password, ref CustomerId))
                {
                    String CustomerStr;
                    CustomerStr = CustomerId.ToString() + ":" + "Admin";

                    FormsAuthentication.SetAuthCookie(CustomerStr, false);

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(user);

        }

        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "AdminLogin");
        }
    }
}