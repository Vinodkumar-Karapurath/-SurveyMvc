using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MtsSurvey.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult CustomerIndex()
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            ViewBag.CompanyId = new SelectList(SurveyContextObj.DbCompany, "CompanyId", "CompanyName");
            return View();
        }

        [HttpPost]
        public ActionResult AddCustomer(CustomerModel CustomerModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            CustomerMaster CustomerObj = new CustomerMaster() { CompanyId = CustomerModelObj.CompanyId, CustomerName = CustomerModelObj.CustomerName, Email = CustomerModelObj.Email, passcode = CustomerModelObj.passcode};
            SurveyContextObj.DbCustomerMaster.Add(CustomerObj);
            SurveyContextObj.SaveChanges();
            CustomerModelObj.CustomerId = CustomerObj.CustomerId;
            CustomerModelObj.CompanyName = SurveyContextObj.DbCompany.Where(p => p.CompanyId == CustomerObj.CompanyId).FirstOrDefault().CompanyName;
            return Json(CustomerModelObj);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(CustomerModel CustomerModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            CustomerMaster CustomerObj = SurveyContextObj.DbCustomerMaster.Find(CustomerModelObj.CustomerId);
            CustomerObj.CompanyId = CustomerModelObj.CompanyId;
            CustomerObj.CustomerName = CustomerModelObj.CustomerName;
            CustomerObj.Email = CustomerModelObj.Email;
            CustomerObj.passcode = CustomerModelObj.passcode;
            SurveyContextObj.Entry(CustomerObj).State = System.Data.Entity.EntityState.Modified;
            SurveyContextObj.SaveChanges();
            CustomerModelObj.CompanyName = SurveyContextObj.DbCompany.Where(p => p.CompanyId == CustomerObj.CompanyId).FirstOrDefault().CompanyName;
            return Json(CustomerModelObj);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(CustomerModel CustomerModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            CustomerMaster CustomerObj = SurveyContextObj.DbCustomerMaster.Find(CustomerModelObj.CustomerId);
            SurveyContextObj.Entry(CustomerObj).State = System.Data.Entity.EntityState.Deleted;
            SurveyContextObj.SaveChanges();
            return Json(CustomerModelObj);
        }

        // GET: ActivityList
        public ActionResult GetAllCustomer()
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            var ActivityList = SurveyContextObj.DbCustomerMaster.Select(p => new {  p.CompanyId, p.CustomerId, p.CustomerName, p.NavCompany.CompanyName , p.Email,p.passcode});
            //var ActivityList = new SelectList(SurveyContextObj.DbCustomerMaster, "CustomerId", "CompanyId", "CustomerName", "Email", "passcode", "SurveyStatus");
            return Json(ActivityList, JsonRequestBehavior.AllowGet);
        }
    }
}