using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MtsSurvey.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SurveyMasterController : Controller
    {
        // GET: SurveyTemplate
        public ActionResult SurveyMasterIndex()
        {
      
            SurveyContext SurveyContextObj = new SurveyContext();
            SurveyTemplate SurveyTemplateObj = new SurveyTemplate();
            SurveyTemplateObj.DateStart = DateTime.Now.Date;
            SurveyTemplateObj.DateEnd= DateTime.Now.Date;
            ViewBag.CompanyBag =  new SelectList(SurveyContextObj.DbCompany, "CompanyId", "CompanyName");
            return View(SurveyTemplateObj);
        }

        [HttpPost]
        public ActionResult AddSurvey(SurveyTemplate SurveyTemplateObj)
        {
            try
            {
                SurveyContext SurveyContextObj = new SurveyContext();
                SurveyMaster SurveyMasterObj = new SurveyMaster();
               
                    SurveyMasterObj.SurveyCaption = SurveyTemplateObj.SurveyCaption;
                    SurveyMasterObj.DateCreated = DateTime.Today;
                    SurveyMasterObj.DateStart = SurveyTemplateObj.DateStart;
                    SurveyMasterObj.DateEnd = SurveyTemplateObj.DateEnd;
                    SurveyMasterObj.NavSurveyQuestion = null;

                 SurveyContextObj.DbSurveyMaster.Add(SurveyMasterObj);
                SurveyContextObj.SaveChanges();
                SurveyTemplateObj.SurveyId = SurveyMasterObj.SurveyId;

                foreach (var SurveyCustomerTemplate in SurveyTemplateObj.SurveyCustomerTemplateModels)
                {
                    SurveyCustomerMap SurveyCustomerMapObj = new SurveyCustomerMap()
                    {
                        SurveyId = SurveyMasterObj.SurveyId,
                        CustomerId = SurveyCustomerTemplate.CustomerId,
                        DateDone = DateTime.Today,
                        SurveyStatus = false,
                        SurveyGuid = GetNewGuid()
                    };
                    SurveyContextObj.DbSurveyCustomerMap.Add(SurveyCustomerMapObj);

                }
                SurveyContextObj.SaveChanges();

                return Json(SurveyTemplateObj);
            }
            catch (Exception ex)
            {

                return Json(new { Message = ex.InnerException.Message });
            }
        }

        public String GetNewGuid()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString;
        }

        [HttpPost]
        public ActionResult UpdatedSurvey(SurveyTemplate SurveyTemplateObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            SurveyMaster SurveyMasterObj = SurveyContextObj.DbSurveyMaster.Find(SurveyTemplateObj.SurveyId);
            SurveyMasterObj.SurveyCaption = SurveyTemplateObj.SurveyCaption;
            SurveyMasterObj.DateStart = SurveyTemplateObj.DateStart;
            SurveyMasterObj.DateEnd = SurveyTemplateObj.DateEnd;
            SurveyContextObj.Entry(SurveyMasterObj).State = System.Data.Entity.EntityState.Modified;
            SurveyContextObj.SaveChanges();

            foreach (var SurveyCustomerTemplate in SurveyTemplateObj.SurveyCustomerTemplateModels)
            {
              if(!SurveyCustomerTemplate.DbValueChk && SurveyCustomerTemplate.UserUpdateChk)
              {
                  //Add new
                  SurveyCustomerMap SurveyCustomerMapObj = new SurveyCustomerMap()
                  {
                      SurveyId = SurveyMasterObj.SurveyId,
                      CustomerId = SurveyCustomerTemplate.CustomerId,
                      DateDone = DateTime.Now,
                      SurveyStatus = false,
                      SurveyGuid = GetNewGuid()
                  };
                  SurveyContextObj.DbSurveyCustomerMap.Add(SurveyCustomerMapObj);
              }
              else if(SurveyCustomerTemplate.DbValueChk && !SurveyCustomerTemplate.UserUpdateChk)
              {
                  //Remove
                  SurveyCustomerMap SurveyCustomerMapObj = SurveyContextObj.DbSurveyCustomerMap.Find(SurveyTemplateObj.SurveyId, SurveyCustomerTemplate.CustomerId);
                  SurveyContextObj.Entry(SurveyCustomerMapObj).State = System.Data.Entity.EntityState.Deleted;
              }
                
            }
            SurveyContextObj.SaveChanges();

            return Json(SurveyTemplateObj);
        }

        [HttpPost]
        public ActionResult DeleteSurvey(SurveyTemplate SurveyTemplateObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();

            List<SurveyCustomerMap> SurveyCustomerMapObj = SurveyContextObj.DbSurveyCustomerMap.Where(p => p.SurveyId == SurveyTemplateObj.SurveyId).ToList();
            SurveyContextObj.DbSurveyCustomerMap.RemoveRange(SurveyCustomerMapObj);

            SurveyMaster SurveyMasterObj = SurveyContextObj.DbSurveyMaster.Find(SurveyTemplateObj.SurveyId);
            SurveyContextObj.Entry(SurveyMasterObj).State = System.Data.Entity.EntityState.Deleted;
            SurveyContextObj.SaveChanges();
            return Json(SurveyTemplateObj);
        }

        // GET: ActivityList
        public ActionResult GetAllSurvey()
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            var ActivityList = SurveyContextObj.DbSurveyMaster.Select(p => new { p.SurveyId, p.SurveyCaption, p.DateStart, p.DateEnd });
            return Json(ActivityList, JsonRequestBehavior.AllowGet);
        }


        // GET: ActivityList
        [HttpPost]
        public ActionResult GetAllCustomerSurveyMap(int SurveyId)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            SurveyTemplate SurveyTemplateObj = new SurveyTemplate();

            if (SurveyId == 0)
            {
                SurveyTemplateObj.SurveyId = 0;
                SurveyTemplateObj.SurveyCaption = "";

                var SurveyCustomerList = SurveyContextObj.DbCustomerMaster.Select(p => new SurveyCustomerTemplate()
                {
                    CompanyId = p.CompanyId,
                    CustomerId = p.CustomerId,
                    CustomerName = p.CustomerName,
                    DbValueChk = false,
                    SurveyStatus = false,
                    UserUpdateChk = false
                });

                SurveyTemplateObj.SurveyCustomerTemplateModels.AddRange(SurveyCustomerList);
            }
            else
            {
                SurveyMaster SurveyMasterObj = SurveyContextObj.DbSurveyMaster.Where(p => p.SurveyId == SurveyId).FirstOrDefault();
                if (SurveyMasterObj != default(SurveyMaster))
                {
                    SurveyTemplateObj.SurveyId = SurveyMasterObj.SurveyId;
                    SurveyTemplateObj.SurveyCaption = SurveyMasterObj.SurveyCaption;
                    SurveyTemplateObj.DateStart = SurveyMasterObj.DateStart;
                    SurveyTemplateObj.DateEnd = SurveyMasterObj.DateEnd;

                    var SurveyCustomerList = from CM in SurveyContextObj.DbCustomerMaster
                                             join SM in SurveyContextObj.DbSurveyCustomerMap.Where(p => p.SurveyId == SurveyId) on CM.CustomerId equals SM.CustomerId into Sr
                                             from Sx in Sr.DefaultIfEmpty()
                                             select new SurveyCustomerTemplate()
                                             {
                                                 CompanyId = CM.CompanyId,
                                                 CustomerId = CM.CustomerId,
                                                 CustomerName = CM.CustomerName,
                                                 DbValueChk = (Sx == null ? false : true),
                                                 UserUpdateChk = (Sx == null ? false : true)
                                             };
                    SurveyTemplateObj.SurveyCustomerTemplateModels.AddRange(SurveyCustomerList);
                }

            }
            return Json(SurveyTemplateObj, JsonRequestBehavior.AllowGet);
        }
    }
}