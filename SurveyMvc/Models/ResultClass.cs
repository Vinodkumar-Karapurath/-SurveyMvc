using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyMvc.Models.Result
{
    public class ResultClass
    {
        public int SurveyId { get; set; }
        public String SurveyCaption { get; set; }
        public String DateStart { get; set; }
        public String DateEnd { get; set; }

        public List<ResultSQ> ResultSQs { get; set; }
    }

    public class ResultSQ
    {
        public int QuestionId { get; set; }
        public int SurveySeq { get; set; }
        public int SurveyType { get; set; }
        public String Surveyquestion { get; set; }

        public List<NoteClass> SurveyNote { get; set; }

        public ChartClass ChartData { get; set; }
    }

    public class NoteClass
    {
        public String CustomerName { get; set; }
        public String CustomerNote { get; set; }
    }

    public class ChartClass
    {
        public List<String> labels { get; set; }
        public List<datasetsClass> datasets { get; set; }
    }

    public class datasetsClass
    {
        public String label { get; set; }
        public String fillColor { get; set; }
        public String strokeColor { get; set; }
        public String highlightFill { get; set; }
        public String highlightStroke { get; set; }
        public List<int> data { get; set; }
    }


}