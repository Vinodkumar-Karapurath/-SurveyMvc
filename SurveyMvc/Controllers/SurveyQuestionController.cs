using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MtsSurvey.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SurveyQuestionController : Controller
    {
        // GET: SurveyQuestion
        public ActionResult SurveyQuestionIndex()
        {
            List<SurveyType> SurveyTypeObj = new List<SurveyType>() {  new SurveyType{ SurveyText = "Options", SurveyTypeId = 1}, 
            new SurveyType{ SurveyText = "TextBox", SurveyTypeId = 2}};

           
            SurveyContext SurveyContextObj = new SurveyContext();
            ViewBag.SurveyTypeBag = new SelectList(SurveyTypeObj, "SurveyTypeId", "SurveyText");
            ViewBag.QuestionBag = new SelectList(SurveyContextObj.DbSurveyAnswerMaster, "AnswerID", "Caption");
            ViewBag.SurveyMasterBag = new SelectList(SurveyContextObj.DbSurveyMaster, "SurveyId", "SurveyCaption");
            return View();           
        }

         [HttpPost]
        public ActionResult AddQuestion(SurveyQuestionVM SurveyQuestionVMObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            SurveyQuestion SurveyQuestionObj = new SurveyQuestion() { SurveyId = SurveyQuestionVMObj.SurveyId, Surveyquestion = SurveyQuestionVMObj.Surveyquestion, SurveySeq = SurveyQuestionVMObj.SurveySeq, SurveyType =SurveyQuestionVMObj.SurveyType,
                                             PossibleAnswersID =   SurveyQuestionVMObj.PossibleAnswersID.HasValue? (int)SurveyQuestionVMObj.PossibleAnswersID: 0};

            SurveyContextObj.DbSurveyQuestion.Add(SurveyQuestionObj);
            SurveyContextObj.SaveChanges();
            SurveyQuestionVMObj.QuestionId = SurveyQuestionObj.QuestionId;
            SurveyAnswerMaster SurveyAnswerMasterObj = SurveyContextObj.DbSurveyAnswerMaster.Where(p => p.AnswerID == SurveyQuestionObj.PossibleAnswersID).FirstOrDefault();
            SurveyQuestionVMObj.PossibleAnswers = SurveyAnswerMasterObj == null? "None" : SurveyAnswerMasterObj.Caption;
            return Json(SurveyQuestionVMObj);
        }

        [HttpPost]
        public ActionResult UpdateQuestion(SurveyQuestionVM SurveyQuestionVMObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
           SurveyQuestion SurveyQuestionObj = SurveyContextObj.DbSurveyQuestion.Find(SurveyQuestionVMObj.QuestionId);

            SurveyQuestionObj.SurveyId = SurveyQuestionVMObj.SurveyId;
            SurveyQuestionObj.SurveySeq = SurveyQuestionVMObj.SurveySeq;
            SurveyQuestionObj.SurveyType = SurveyQuestionVMObj.SurveyType;
            SurveyQuestionObj.Surveyquestion = SurveyQuestionVMObj.Surveyquestion;
            if (SurveyQuestionVMObj.SurveyType == 2) SurveyQuestionVMObj.PossibleAnswersID = null;
            SurveyQuestionObj.PossibleAnswersID = SurveyQuestionVMObj.PossibleAnswersID.HasValue? (int)SurveyQuestionVMObj.PossibleAnswersID: 0;

            SurveyContextObj.Entry(SurveyQuestionObj).State = System.Data.Entity.EntityState.Modified;
            SurveyContextObj.SaveChanges();
            SurveyAnswerMaster SurveyAnswerMasterObj = SurveyContextObj.DbSurveyAnswerMaster.Where(p => p.AnswerID == SurveyQuestionObj.PossibleAnswersID).FirstOrDefault();
            SurveyQuestionVMObj.PossibleAnswers = SurveyAnswerMasterObj == null? "None" : SurveyAnswerMasterObj.Caption;
 
            return Json(SurveyQuestionVMObj);
        }

        [HttpPost]
        public ActionResult DeleteQuestion(SurveyQuestionVM SurveyQuestionVMObj)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
             SurveyQuestion SurveyQuestionObj = SurveyContextObj.DbSurveyQuestion.Find(SurveyQuestionVMObj.QuestionId);
            SurveyContextObj.Entry(SurveyQuestionObj).State = System.Data.Entity.EntityState.Deleted;
            SurveyContextObj.SaveChanges();
            return Json(SurveyQuestionObj);
        }

        // GET: ActivityList
        public ActionResult GetAllQuestion()
        {
            SurveyContext SurveyContextObj = new SurveyContext();
          // var ActivityList1 = SurveyContextObj.DbSurveyQuestion.Select(p => new {  p.QuestionId, p.SurveyId, p.SurveySeq, p.SurveyType , p.Surveyquestion, p.PossibleAnswersID});

             var ActivityList = from Sq in SurveyContextObj.DbSurveyQuestion
                                join Sa in SurveyContextObj.DbSurveyAnswerMaster on Sq.PossibleAnswersID equals Sa.AnswerID into Sr
                                from Sx in Sr.DefaultIfEmpty()
                                select new 
                                {
                                    Sq.QuestionId, Sq.SurveyId, Sq.SurveySeq, Sq.SurveyType , Sq.Surveyquestion, Sq.PossibleAnswersID,
                                    PossibleAnswers = (Sx == null ? string.Empty: Sx.Caption)
                                };

            
            return Json(ActivityList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPossibleAnswers(int PossibleAnswersID)
        {
            SurveyContext SurveyContextObj = new SurveyContext();

            var SurveyAnswerModelObj = SurveyContextObj.DbSurveyAnswer.Where(cp => cp.AnswerID == PossibleAnswersID)
               .Select(p => new SurveyAnswerModel { AnswerSeq = p.AnswerSeq, AnswerText = p.AnswerText });

            return Json(SurveyAnswerModelObj, JsonRequestBehavior.AllowGet);
        }
    }
}