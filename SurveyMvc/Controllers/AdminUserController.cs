using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        // GET: Company
        public ActionResult AdminUserIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAdminUser(AdminuserModel AdminuserModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            AdminLogin AdminLoginObj = new AdminLogin() { Email = AdminuserModelObj.Email, passcode = AdminuserModelObj.passcode };
            SurveyContextObj.DbAdminLogin.Add(AdminLoginObj);
            SurveyContextObj.SaveChanges();
            AdminuserModelObj.AdminLoginId = AdminLoginObj.AdminLoginId;
            return Json(AdminuserModelObj);
        }

        [HttpPost]
        public ActionResult UpdateAdminUser(AdminuserModel AdminuserModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            AdminLogin AdminLoginObj = SurveyContextObj.DbAdminLogin.Find(AdminuserModelObj.AdminLoginId);
            AdminLoginObj.Email = AdminuserModelObj.Email;
            AdminLoginObj.passcode = AdminuserModelObj.passcode;
            SurveyContextObj.Entry(AdminLoginObj).State = System.Data.Entity.EntityState.Modified;
            SurveyContextObj.SaveChanges();
            return Json(AdminuserModelObj);
        }

        [HttpPost]
        public ActionResult DeleteAdminUser(AdminuserModel AdminuserModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            AdminLogin AdminLoginObj = SurveyContextObj.DbAdminLogin.Find(AdminuserModelObj.AdminLoginId);
            SurveyContextObj.Entry(AdminLoginObj).State = System.Data.Entity.EntityState.Deleted;
            SurveyContextObj.SaveChanges();
            return Json(AdminuserModelObj);
        }

        // GET: ActivityList
        public ActionResult GetAllAdminUser()
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            var ActivityList = SurveyContextObj.DbAdminLogin.Select(p => new { p.AdminLoginId, p.Email, p.passcode }).ToList();
            return Json(ActivityList, JsonRequestBehavior.AllowGet);
        }
    }
}