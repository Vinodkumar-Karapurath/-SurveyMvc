using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class SurveyCommonTask
    {
        public static int CreateSurveyModel(int CustomerId, int _Surveyid, ref UserVM model)
        {
                     
            SurveyContext SurveyContextObj = new SurveyContext();
            CustomerMaster CustomerMasterObj = SurveyContextObj.DbCustomerMaster.Where(sa => sa.CustomerId == CustomerId).FirstOrDefault();
            SurveyCustomerMap SurveyCustomerMapObj;

            if (_Surveyid == 0) // take first active survey
            {
                SurveyCustomerMapObj = SurveyContextObj.DbSurveyCustomerMap.Where(p => p.CustomerId == CustomerId && p.SurveyStatus == false && p.NavSurveyMaster.DateStart <= DateTime.Now && p.NavSurveyMaster.DateEnd >= DateTime.Now).FirstOrDefault();
            }
            else
            {
                SurveyCustomerMapObj = SurveyContextObj.DbSurveyCustomerMap.Where(p => p.CustomerId == CustomerId && p.SurveyId == _Surveyid && p.SurveyStatus == false && p.NavSurveyMaster.DateStart <= DateTime.Now && p.NavSurveyMaster.DateEnd >= DateTime.Now).FirstOrDefault();

            }
            if (SurveyCustomerMapObj == default(SurveyCustomerMap))
            {
                return 1; // survay completed.
            }


            model.UserID = CustomerMasterObj.CustomerId;
            model.Name = CustomerMasterObj.CustomerName;
            model.NavQuestions = new List<QuestionVM>();

            model.SurveyID = SurveyCustomerMapObj.SurveyId;
            model.SurveyCaption = SurveyCustomerMapObj.NavSurveyMaster.SurveyCaption;
            var SurveyQuestObjs = SurveyContextObj.DbSurveyQuestion.Where(p => p.SurveyId == SurveyCustomerMapObj.SurveyId).ToList();

            foreach (var SurveyQuestObj in SurveyQuestObjs)
            {
                QuestionVM QuestionVMObj = new QuestionVM();
                QuestionVMObj.ID = SurveyQuestObj.QuestionId;
                QuestionVMObj.Text = SurveyQuestObj.Surveyquestion;
                QuestionVMObj.QuestionType = SurveyQuestObj.SurveyType;

                List<AnswerVM> PossibleAnswers = new List<AnswerVM>();

                var SurveyAnswerObjs = SurveyContextObj.DbSurveyAnswer.Where(s => s.AnswerID == SurveyQuestObj.PossibleAnswersID).ToList();
                foreach (var SurveyAnswerObj in SurveyAnswerObjs)
                {
                    AnswerVM AnswerVMObj = new AnswerVM();
                    AnswerVMObj.ID = SurveyAnswerObj.AnswerSeq;
                    AnswerVMObj.Text = SurveyAnswerObj.AnswerText;
                    PossibleAnswers.Add(AnswerVMObj);
                }
                QuestionVMObj.NavPossibleAnswers = PossibleAnswers;
                model.NavQuestions.Add(QuestionVMObj);
            }

            return 0; // survey ready.
        }

        public static bool SaveSurveyModel(UserVM model)
        {
            //try
           {
                SurveyContext SurveyContextObj = new SurveyContext();

                foreach (QuestionVM SurveyRst in model.NavQuestions)
                {
                    SurveyResult SurveyResultObj = new SurveyResult();
                    SurveyResultObj.CustomerId = model.UserID;
                    SurveyResultObj.SurveyId = model.SurveyID;
                    SurveyResultObj.QuestionId =  SurveyRst.ID;
                    SurveyResultObj.SurveyPoint = SurveyRst.SelectedAnswer.HasValue ? (int)SurveyRst.SelectedAnswer : 0;
                    SurveyResultObj.SurveyReply = SurveyRst.SurveyReply;
                    SurveyResultObj.SurveyDate = DateTime.Now;
                    SurveyContextObj.DbSurveyResult.Add(SurveyResultObj);
                
                }
              
                SurveyCustomerMap SurveyCustomerMapObj = SurveyContextObj.DbSurveyCustomerMap.Find(model.SurveyID, model.UserID); // Where(p => p.CustomerId == CustomerId && p.SurveyId == _Surveyid && p.SurveyStatus == false && p.NavSurveyMaster.DateStart <= DateTime.Now && p.NavSurveyMaster.DateEnd >= DateTime.Now).FirstOrDefault();
                SurveyCustomerMapObj.SurveyStatus = true;
                SurveyContextObj.Entry(SurveyCustomerMapObj).State = System.Data.Entity.EntityState.Modified;
                SurveyContextObj.SaveChanges();
                return true;
            }
            //catch
            {
             //   return false;
            }
        }
    }
}