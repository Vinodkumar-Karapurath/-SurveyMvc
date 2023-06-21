using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace SurveyMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    //try
                    //{

                        //let us take out the username now                
                        string CookieName = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        String[] CustomerStrSpl = CookieName.Split(':');

                        int CustId = int.Parse(CustomerStrSpl[0]);
                        string username = "";

                        using ( SurveyContext SurveyContextObj = new SurveyContext())
                        {
                            if(CustomerStrSpl[1] == "Local")
                            {

                                username = SurveyContextObj.DbCustomerMaster.Where(p => p.CustomerId == CustId).FirstOrDefault().CustomerName;

                            }
                            else if(CustomerStrSpl[1] == "Admin")
                            {
                                username = SurveyContextObj.DbAdminLogin.Where(p => p.AdminLoginId == CustId).FirstOrDefault().Email; // need to chage Email
                            }
                            
                        }

                        //let us extract the roles from our own custom cookie


                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                             new System.Security.Principal.GenericIdentity(username, "Forms"), CustomerStrSpl[1].Split(';'));

                    //}
                    //catch (Exception)
                    //{
                    //    //somehting went wrong
                    //}
                }
            }
        }

    }
}
