using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MtsSurvey.Controllers
{
    //[Authorize(Roles = "Local, Admin")]
    public class SurveyController : Controller
    {
        // GET: Survey
        public ActionResult SurveyIndex(int _Argsurveyid = 0, int _ArgCustId = 0)
        {
            // populate your view model with values from the database
            UserVM model = new UserVM();
            int result = SurveyCommonTask.CreateSurveyModel(_ArgCustId, _Argsurveyid, ref model);

            if (result == 1)
            {
                return RedirectToAction("CompleteSurvey", "Survey");
            }
            else
            {
                return View(model); 
            }

           
        }

        // GET: Survey
        public ActionResult CompleteSurvey()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SurveyIndex(UserVM model)
        {
            // save and redirect
            SurveyCommonTask.SaveSurveyModel(model);
            FormsAuthentication.SignOut();
            return RedirectToAction("CompleteSurvey", "Survey");
        }
    }
}