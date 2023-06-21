using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class SurveyContext : DbContext
    {
        public SurveyContext()
            : base("name = SurveyDb")
        {
           // Database.SetInitializer<SurveyContext>(new DropCreateDatabaseIfModelChanges<SurveyContext>());

            Database.SetInitializer<SurveyContext>(new AdminLoginInitializer());
        }

        public DbSet<AdminLogin> DbAdminLogin { get; set; }
        public DbSet<Company> DbCompany { get; set; }
        public DbSet<CustomerMaster> DbCustomerMaster { get; set; }
        public DbSet<SurveyMaster> DbSurveyMaster { get; set; }
        public DbSet<SurveyCustomerMap> DbSurveyCustomerMap { get; set; }
        public DbSet<SurveyQuestion> DbSurveyQuestion { get; set; }
        public DbSet<SurveyAnswerMaster> DbSurveyAnswerMaster { get; set; }
        public DbSet<SurveyAnswer> DbSurveyAnswer { get; set; }
        public DbSet<SurveyResult> DbSurveyResult { get; set; }

       
    }

    /// <summary>
    /// AdminLogin seed
    /// </summary>
    public class AdminLoginInitializer : DropCreateDatabaseIfModelChanges<SurveyContext>
    {
        protected override void Seed(SurveyContext context)
        {
            AdminLogin defaultStandards = new AdminLogin();
            defaultStandards.Email = "Admin";
            defaultStandards.passcode = 11223;
            context.DbAdminLogin.Add(defaultStandards);

            base.Seed(context);
        }
    }

}