using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyMvc.Controllers
{
    public class HelpController : Controller
    {
        // GET: Help
        public ActionResult HelpIndex()
        {
            return View();
        }
    }
}