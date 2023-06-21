using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    /// <summary>
    /// AdminLogin
    /// </summary>
    public class AdminLogin
    {
        public AdminLogin() { }

        [Key]
        public int AdminLoginId { get; set; }
        [StringLength(50)]
        public String Email { get; set; }
        public int passcode { get; set; }

    }
        

    /// <summary>
    /// Company
    /// </summary>
    public class Company
    {
        public Company() {
            NavCustomerMaster = new List<CustomerMaster>();
        }

        [Key]
        public int CompanyId { get; set; }
        [StringLength(50)]
        public String CompanyName { get; set; }

        public virtual ICollection<CustomerMaster> NavCustomerMaster { get; set; }
    }
    /// <summary>
    /// CustomerMaster
    /// </summary>
    public class CustomerMaster
    {
        public CustomerMaster() { }

        [Key]
        public int CustomerId { get; set; }
        public int CompanyId { get; set; }

        [StringLength(50)]
        public String CustomerName { get; set; }
        [StringLength(50)]
        public String Email { get; set; }
        public int passcode { get; set; }
       

         [ForeignKey("CompanyId")]
        public virtual Company NavCompany { get; set; }
    }
    /// <summary>
    /// SurveyMaster
    /// </summary>
    public class SurveyMaster
    {
        public SurveyMaster() 
        {
        }

        [Key]
        public int SurveyId { get; set; }
        [StringLength(150)]
        public String SurveyCaption { get; set; }
        public DateTime DateCreated { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }

        public virtual ICollection<SurveyQuestion> NavSurveyQuestion { get; set; }

    }

     /// <summary>
    /// SurveyCustomerMap
    /// </summary>
    public class SurveyCustomerMap
    {
        public SurveyCustomerMap()
        {
        }
        [Key]
        [Column(Order = 1)]
        public int SurveyId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int CustomerId { get; set; }
        public DateTime DateDone { get; set; }
        public bool SurveyStatus { get; set; }

        public String SurveyGuid { get; set; }

        [ForeignKey("SurveyId")]
        public virtual SurveyMaster NavSurveyMaster { get; set; }

        [ForeignKey("CustomerId")]
        public virtual CustomerMaster NavCustomerMaster { get; set; }

    }

    /// <summary>
    /// SurveyMaster
    /// </summary>
    public class SurveyQuestion
    {
        public SurveyQuestion()
        {
        }

        [Key]
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public int SurveySeq { get; set; }
        public int SurveyType { get; set; }
        [StringLength(150)]
        public String Surveyquestion { get; set; }

        public int PossibleAnswersID { get; set; }

        [ForeignKey("SurveyId")]
        public virtual SurveyMaster NavSurveyMaster { get; set; }

    }

     /// <summary>
    /// SurveyAnswer PossibleAnswers
    /// </summary>
    public class SurveyAnswerMaster
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AnswerID { get; set; }
        public string Caption { get; set; }
    }
    /// <summary>
    /// SurveyAnswer PossibleAnswers
    /// </summary>
    public class SurveyAnswer
    {
        [Key]
        [Column(Order = 1)]
        public int AnswerID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int AnswerSeq { get; set; }
        [StringLength(150)]
        public string AnswerText { get; set; }

    }

    /// <summary>
    /// SurveyResult
    /// </summary>
    public class SurveyResult
    {
        public SurveyResult() { }

        [Key]
        [Column(Order = 1)]
        public int CustomerId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int SurveyId { get; set; }

        [Key]
        [Column(Order = 3)]
        public int QuestionId { get; set; }

        public DateTime SurveyDate { get; set; }

        public int SurveyPoint { get; set; }
        [StringLength(500)]
        public String SurveyReply { get; set; }

        [ForeignKey("CustomerId")]
        public virtual CustomerMaster NavCustomerMaster { get; set; }

        //[ForeignKey("SurveyId")]
        //public virtual SurveyMaster NavSurveyMaster { get; set; }

        [ForeignKey("QuestionId")]
        public virtual SurveyQuestion NavSurveyQuestion { get; set; }
    }

}