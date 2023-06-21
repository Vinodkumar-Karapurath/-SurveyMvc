using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class AdminuserModel
    {
        public int AdminLoginId { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "The Email is required.")]
        public String Email { get; set; }
        public int passcode { get; set; }
    }
}