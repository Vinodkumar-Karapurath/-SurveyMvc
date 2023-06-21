using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace SurveyMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SurveyEmailController : Controller
    {
        // GET: SurveyEmail
        public ActionResult SurveyEmailIndex()
        {
            EmailTemplate EmailTemplateObj = new EmailTemplate();

            SurveyContext SurveyContextObj = new SurveyContext();

            ViewBag.SurveyBag = new SelectList(SurveyContextObj.DbSurveyMaster, "SurveyId", "SurveyCaption");
            ViewBag.CompanyBag = new SelectList(SurveyContextObj.DbCompany, "CompanyId", "CompanyName");

            EmailTemplateObj.EmailMsg = "We request you to take part in the Customer Satisfaction Survey. As we are constantly attempting to improve in every area, your inputs would be of great value to us.";
            return View(EmailTemplateObj);
        }

        /// <summary>
        ///  SendEmail to customers
        /// </summary>
        /// <param name="EmailTemplateObj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendEmail(EmailTemplate EmailTemplateObj)
        {
            // Gmail Address from where you send the mail
            var fromAddress = "KK@gmail.com";
            //Password of your gmail address
            const string fromPassword = "XXXXXXX";

            SurveyContext SurveyContextObj = new SurveyContext();

            foreach (var EmailSurveyCustomerMapobj in EmailTemplateObj.EmailSurveyCustomerMapModels)
            {
                CustomerMaster CustomerMasterObj = SurveyContextObj.DbCustomerMaster.Find(EmailSurveyCustomerMapobj.CustomerId);
                SurveyCustomerMap SurveyCustomerMapObj = SurveyContextObj.DbSurveyCustomerMap.Find(EmailTemplateObj.SurveyId, EmailSurveyCustomerMapobj.CustomerId);

                using (MailMessage mm = new MailMessage(fromAddress, CustomerMasterObj.Email))
                {

                    string Userurl = Url.Action("EmailLogin", "Login",
                        new System.Web.Routing.RouteValueDictionary(new { SurveyGuid = SurveyCustomerMapObj.SurveyGuid }),
                        "http", Request.Url.Host);

                    mm.Subject = "Mirnah Customer Survey";
                    string body = "Dear " + CustomerMasterObj.CustomerName + ", \n";

                    body += EmailTemplateObj.EmailMsg;
                    body += "\n";
                    body += Userurl;
                    body += "\n";
                    body += " Thanks & Regards, \n Mirnah Technology systems \n";
                   
                    mm.Body = body;

                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "pop.gmail.com";
                    smtp.EnableSsl = false;
                    NetworkCredential NetworkCred = new NetworkCredential(fromAddress, fromPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }

            }

            return Json(EmailTemplateObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get selected Survey Details
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCustomerSurveyMap(int SurveyId)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            EmailTemplate EmailTemplateObj = new EmailTemplate();

            if (SurveyId != 0)
            {
                SurveyMaster SurveyMasterObj = SurveyContextObj.DbSurveyMaster.Where(p => p.SurveyId == SurveyId).FirstOrDefault();
                if (SurveyMasterObj != default(SurveyMaster))
                {
                    EmailTemplateObj.SurveyId = SurveyMasterObj.SurveyId;
                    EmailTemplateObj.SurveyCaption = SurveyMasterObj.SurveyCaption;
                    EmailTemplateObj.DateStart = SurveyMasterObj.DateStart.ToString("dd/MMM/yyyy");
                    EmailTemplateObj.DateEnd = SurveyMasterObj.DateEnd.ToString("dd/MMM/yyyy");

                    var EmailSurveyCustomerList = from SM in SurveyContextObj.DbSurveyCustomerMap.Where(p => p.SurveyId == SurveyId && p.SurveyStatus == false)
                                                  join CM in SurveyContextObj.DbCustomerMaster on SM.CustomerId equals CM.CustomerId into SC
                                                  select new EmailSurveyCustomerMap()
                                                  {
                                                      CompanyId = SM.NavCustomerMaster.CompanyId,
                                                      CustomerId = SM.CustomerId,
                                                      CustomerName = SM.NavCustomerMaster.CustomerName,
                                                      UserUpdateChk = false
                                                  };

                    EmailTemplateObj.EmailSurveyCustomerMapModels.AddRange(EmailSurveyCustomerList);
                }

            }
            return Json(EmailTemplateObj, JsonRequestBehavior.AllowGet);
        }

    }
}