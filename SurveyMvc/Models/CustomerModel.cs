using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "The Company is required.")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Display(Name = "CompanyName")]
        public String CompanyName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "The CustomerName is required.")]
        [Display(Name = "Customer Name")]
        public String CustomerName { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "The Email is required.")]
        public String Email { get; set; }
        public int passcode { get; set; }
    }
}