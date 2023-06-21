using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace MtsSurvey.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("SurveyIndex", "Survey");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserLoginModel user)
        {
            if (ModelState.IsValid)
            {
                int CustomerId = 0;
                if (user.IsValidCustomer(user.UserEmail, user.Password, ref CustomerId))
                {
                    String CustomerStr;
                    CustomerStr = CustomerId.ToString() + ":" + "Local";

                    FormsAuthentication.SetAuthCookie(CustomerStr, false);

                    return RedirectToAction("SurveyIndex", new RouteValueDictionary(new { controller = "Survey", action = "SurveyIndex", _Argsurveyid = 0, _ArgCustId = CustomerId }));

                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(user);
        
        }

        [HttpGet]
        public ActionResult EmailLogin(String SurveyGuid)
        {
            int SurveyId = 0;
            string UserEmail = "";
            int Password = 0;
            SurveyContext SurveyContextObj = new SurveyContext();

            SurveyCustomerMap SurveyCustomerMapObj = SurveyContextObj.DbSurveyCustomerMap.Where(p => p.SurveyGuid == SurveyGuid).FirstOrDefault();

            if(SurveyCustomerMapObj != default(SurveyCustomerMap))
            {
                SurveyId = SurveyCustomerMapObj.SurveyId;
                CustomerMaster CustomerMasteObj = SurveyContextObj.DbCustomerMaster.Find(SurveyCustomerMapObj.CustomerId);
                UserEmail = CustomerMasteObj.Email;
                Password = CustomerMasteObj.passcode;
            }

            UserLoginModel user = new UserLoginModel() { UserEmail = UserEmail, Password = Password };
            int CustomerId = 0;
            if (user.IsValidCustomer(user.UserEmail, user.Password, ref CustomerId))
            {
                String CustomerStr;
                CustomerStr = CustomerId.ToString() + ":" + "Local";

                FormsAuthentication.SetAuthCookie(CustomerStr, false);

                return RedirectToAction("SurveyIndex", new RouteValueDictionary(new { controller = "Survey", action = "SurveyIndex", _Argsurveyid = SurveyId, _ArgCustId = CustomerId }));

            }
            else
            {
                ModelState.AddModelError("", "Login data is incorrect!");
            }

            return View();
        }


        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}