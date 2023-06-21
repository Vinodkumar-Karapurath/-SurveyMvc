using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class UserVM
    {
        public int UserID { get; set; }
        public int SurveyID { get; set; }
        public string Name { get; set; }
        public string SurveyCaption { get; set; }
        // plus any other properties of Question that you want to display in the view
        public List<QuestionVM> NavQuestions { get; set; }
    }

    public class QuestionVM
    {
        public int ID { get; set; } // for binding
        public string Text { get; set; }
        public int QuestionType { get; set; }
        [Required]
        public int? SelectedAnswer { get; set; } // for binding
        [StringLength(500)]
        public String SurveyReply { get; set; } // for binding

        public IEnumerable<AnswerVM> NavPossibleAnswers { get; set; }
    }

    public class AnswerVM
    {
        public int ID { get; set; }
        public string Text { get; set; }
    }

   
}