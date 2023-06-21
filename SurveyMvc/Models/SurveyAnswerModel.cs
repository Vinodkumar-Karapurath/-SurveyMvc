using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class SurveyAnsMasterModel
    {
        public int AnswerID { get; set; }
        [StringLength(150)]
        [Required(ErrorMessage = "The Answer Caption is required.")]
        [Display(Name = "Answer Caption")]
        public string Caption { get; set; }

        public List<SurveyAnswerModel> SurveyAnswerModels { get { return _SurveyAnswerModels; } }

        private List<SurveyAnswerModel> _SurveyAnswerModels = new List<SurveyAnswerModel>();

    }

    public class SurveyAnswerModel
    {
        public int AnswerSeq { get; set; }
        [StringLength(150)]
        [Required(ErrorMessage = "The Answer Text is required.")]
        [Display(Name = "Answer Text")]
        public string AnswerText { get; set; }

    }
}