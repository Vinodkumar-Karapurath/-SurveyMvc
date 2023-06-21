using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class EmailTemplate
    {
         public int SurveyId { get; set; }
        [StringLength(150)]
        [Display(Name = " Survey Name")]
        public string SurveyCaption { get; set; }
        [Display(Name = "Starting Date")]
        public string DateStart { get; set; }
        [Display(Name = "Ending Date")]
        public string DateEnd { get; set; }

         [Display(Name = "E-mail Message")]
         public string EmailMsg { get; set; }


         public List<EmailSurveyCustomerMap> EmailSurveyCustomerMapModels { get { return _EmailSurveyCustomerMapModels; } }

         private List<EmailSurveyCustomerMap> _EmailSurveyCustomerMapModels = new List<EmailSurveyCustomerMap>();
    }


    /// <summary>
    /// EmailSurveyCustomerMap
    /// </summary>
    public class EmailSurveyCustomerMap
    {
        public int CustomerId { get; set; }
        public String CustomerName { get; set; }
        public int CompanyId { get; set; }

        public bool UserUpdateChk { get; set; }

    }

}