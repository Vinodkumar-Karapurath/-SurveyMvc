using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class SurveyTemplate
    {
        public int SurveyId { get; set; }
        [StringLength(150)]
        [Required(ErrorMessage = "The  Survey is required.")]
        [Display(Name = " Survey Name")]
        public string SurveyCaption { get; set; }
        [Display(Name = "Starting Date")]
        public DateTime DateStart { get; set; }
         [Display(Name = "Ending Date")]
         public DateTime DateEnd { get; set; }

         public string DateStartForDisplay
         {
             get
             {
                 return this.DateStart.ToString("dd/MM/yy");
             }
         }
         public string DateEndForDisplay
         {
             get
             {
                 return this.DateEnd.ToString("dd/MM/yy");
             }
         }


         public List<SurveyCustomerTemplate> SurveyCustomerTemplateModels { get { return _SurveyCustomerTemplateModels; } }

        private List<SurveyCustomerTemplate> _SurveyCustomerTemplateModels = new List<SurveyCustomerTemplate>();
    }

    /// <summary>
    /// SurveyCustomerTemplate
    /// </summary>
    public class SurveyCustomerTemplate
    {
        public int CustomerId { get; set; }
        public String CustomerName { get; set; }
        public int CompanyId { get; set; }
        public bool SurveyStatus { get; set; }

        public bool DbValueChk { get; set; }
        public bool UserUpdateChk { get; set; }

    }


    public class SurveyQuestionVM
    {
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public int SurveySeq { get; set; }
        public int SurveyType { get; set; }
        [StringLength(150)]
        public String Surveyquestion { get; set; }
        public int? PossibleAnswersID { get; set; } // for binding
        public String PossibleAnswers { get; set; }
    }

    public class PossibleQuestionVM
    {
        public int ID { get; set; }
        public string Text { get; set; }
    }

    public class SurveyType
    {
        public int SurveyTypeId { get; set; }
        public String SurveyText { get; set; }

    }
}