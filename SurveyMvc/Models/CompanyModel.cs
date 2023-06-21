using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class CompanyModel
    {
            public int CompanyId { get; set; }
            [StringLength(50)]
            [Required(ErrorMessage = "The CompanyName is required.")]
            [Display(Name ="Company Name")]
            public String CompanyName { get; set; }     
    }
}