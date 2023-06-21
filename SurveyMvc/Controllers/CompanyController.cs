using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MtsSurvey.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult CompanyIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCompany(CompanyModel CompanyModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            Company companyObj = new Company(){ CompanyName = CompanyModelObj.CompanyName};
            SurveyContextObj.DbCompany.Add(companyObj);
            SurveyContextObj.SaveChanges();
            CompanyModelObj.CompanyId = companyObj.CompanyId;
            return Json(CompanyModelObj);
        }

        [HttpPost]
        public ActionResult UpdateCompany(CompanyModel CompanyModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            Company companyObj = SurveyContextObj.DbCompany.Find(CompanyModelObj.CompanyId);
            companyObj.CompanyName = CompanyModelObj.CompanyName;
            SurveyContextObj.Entry(companyObj).State = System.Data.Entity.EntityState.Modified;
            SurveyContextObj.SaveChanges();
            return Json(CompanyModelObj);
        }

        [HttpPost]
        public ActionResult DeleteCompany(CompanyModel CompanyModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            Company companyObj = SurveyContextObj.DbCompany.Find(CompanyModelObj.CompanyId);
            SurveyContextObj.Entry(companyObj).State = System.Data.Entity.EntityState.Deleted;
            SurveyContextObj.SaveChanges();
            return Json(CompanyModelObj);
        }

        // GET: ActivityList
        public ActionResult GetAllCompany()
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            var ActivityList = SurveyContextObj.DbCompany.Select(p => new { p.CompanyId, p.CompanyName }).ToList();
            return Json(ActivityList, JsonRequestBehavior.AllowGet);
        }
    }
}