using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MtsSurvey.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PossibleOptionController : Controller
    {
        // GET: SurveyAnswer
        public ActionResult PossibleOptionIndex()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult AddSurveyAnswer(SurveyAnsMasterModel SurveyAnsMasterModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            SurveyAnswerMaster SurveyAnswerMasternObj = new SurveyAnswerMaster()
            {
                Caption = SurveyAnsMasterModelObj.Caption              
            };
            SurveyContextObj.DbSurveyAnswerMaster.Add(SurveyAnswerMasternObj);
            SurveyContextObj.SaveChanges();
            SurveyAnsMasterModelObj.AnswerID = SurveyAnswerMasternObj.AnswerID;

             foreach (SurveyAnswerModel SurveyAns in SurveyAnsMasterModelObj.SurveyAnswerModels)
             {
                  SurveyAnswer SurveyAnswerObj = new SurveyAnswer();
                    SurveyAnswerObj.AnswerID = SurveyAnsMasterModelObj.AnswerID;
                    SurveyAnswerObj.AnswerSeq = SurveyAns.AnswerSeq;
                    SurveyAnswerObj.AnswerText =  SurveyAns.AnswerText;
                  SurveyContextObj.DbSurveyAnswer.Add(SurveyAnswerObj);

             }
            SurveyContextObj.SaveChanges();
            var ActivityList = new { AnswerID = SurveyAnsMasterModelObj.AnswerID, Caption = SurveyAnsMasterModelObj.Caption };
            return Json(ActivityList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSurveyAnswer(SurveyAnsMasterModel SurveyAnsMasterModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            SurveyAnswerMaster SurveyAnswerMasternObj = SurveyContextObj.DbSurveyAnswerMaster.Find(SurveyAnsMasterModelObj.AnswerID);

            SurveyAnswerMasternObj.Caption = SurveyAnsMasterModelObj.Caption;
            SurveyContextObj.Entry(SurveyAnswerMasternObj).State = System.Data.Entity.EntityState.Modified;
            SurveyContextObj.SaveChanges();

            SurveyContextObj.DbSurveyAnswer.RemoveRange(SurveyContextObj.DbSurveyAnswer.Where(p => p.AnswerID == SurveyAnsMasterModelObj.AnswerID));

            foreach (SurveyAnswerModel SurveyAns in SurveyAnsMasterModelObj.SurveyAnswerModels)
            {
                SurveyAnswer SurveyAnswerObj = new SurveyAnswer();
                SurveyAnswerObj.AnswerID = SurveyAnsMasterModelObj.AnswerID;
                SurveyAnswerObj.AnswerSeq = SurveyAns.AnswerSeq;
                SurveyAnswerObj.AnswerText = SurveyAns.AnswerText;
                SurveyContextObj.DbSurveyAnswer.Add(SurveyAnswerObj);

            }
            SurveyContextObj.SaveChanges();
            var ActivityList = new { AnswerID = SurveyAnsMasterModelObj.AnswerID, Caption = SurveyAnsMasterModelObj.Caption };
            return Json(ActivityList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteSurveyAnswer(SurveyAnsMasterModel SurveyAnsMasterModelObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            SurveyContextObj.DbSurveyAnswer.RemoveRange(SurveyContextObj.DbSurveyAnswer.Where(p => p.AnswerID == SurveyAnsMasterModelObj.AnswerID));

            SurveyAnswerMaster SurveyAnswerMasternObj = SurveyContextObj.DbSurveyAnswerMaster.Find(SurveyAnsMasterModelObj.AnswerID);
            SurveyContextObj.Entry(SurveyAnswerMasternObj).State = System.Data.Entity.EntityState.Deleted;
            SurveyContextObj.SaveChanges();
            var ActivityList = new { AnswerID = SurveyAnsMasterModelObj.AnswerID, Caption = SurveyAnsMasterModelObj.Caption };
            return Json(ActivityList, JsonRequestBehavior.AllowGet);
        }

        // GET: ActivityList
        public ActionResult GetSelectedSurveyAnswer(int AnswerID)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            SurveyAnsMasterModel SurveyAnsMasterModelObj = new SurveyAnsMasterModel();
            SurveyAnswerMaster SurveyAnswerMasterRec = SurveyContextObj.DbSurveyAnswerMaster.Where(cp => cp.AnswerID ==AnswerID).FirstOrDefault();

            SurveyAnsMasterModelObj.AnswerID = SurveyAnswerMasterRec.AnswerID;
            SurveyAnsMasterModelObj.Caption = SurveyAnswerMasterRec.Caption;

            var SurveyAnswerModelObj = SurveyContextObj.DbSurveyAnswer.Where(cp => cp.AnswerID == AnswerID )
                .Select(p => new SurveyAnswerModel{ AnswerSeq = p.AnswerSeq , AnswerText = p.AnswerText} );

            SurveyAnsMasterModelObj.SurveyAnswerModels.AddRange(SurveyAnswerModelObj);

            return Json(SurveyAnsMasterModelObj, JsonRequestBehavior.AllowGet);
        }

        // GET: ActivityList
        public ActionResult GetAllSurveyAnswer()
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            var ActivityList = SurveyContextObj.DbSurveyAnswerMaster.Select(p => new { p.AnswerID, p.Caption });
            return Json(ActivityList, JsonRequestBehavior.AllowGet);
        }
    }
}