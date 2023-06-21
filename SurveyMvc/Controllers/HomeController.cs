using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
       
            return View();
        }

        [HttpPost]
        public ActionResult GeSurveySummary()
        {
            SurveyContext SurveyContextObj = new SurveyContext();

            var EmailSurveyCustomerList = from SM in SurveyContextObj.DbSurveyMaster.Where(p => p.DateStart <= DateTime.Now && p.DateEnd >= DateTime.Now)
                                          join CSM in SurveyContextObj.DbSurveyCustomerMap on SM.SurveyId equals CSM.SurveyId into SC
                                          from SCM in SC.DefaultIfEmpty()
                                          group SCM by new { SCM.SurveyId, SCM.NavSurveyMaster.SurveyCaption } into g
                                         select new 
                                          {
                                              g.Key.SurveyId,
                                              g.Key.SurveyCaption,
                                              countFinished = g.Count(p => p.SurveyStatus == true),
                                              countRemains = g.Count(p => p.SurveyStatus == false)
                                          };

            return Json(EmailSurveyCustomerList, JsonRequestBehavior.AllowGet);
        }
    }
}