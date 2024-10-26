using Autofac;
using Autofac.Integration.Mvc;
using Loader.Models;
using Loader.Repository;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Rotativa;

namespace Loader
{
    public class MvcApplication : System.Web.HttpApplication
    {
        GenericUnitOfWork uow = new GenericUnitOfWork();
        public static int GUserId { get; set; }
        public static int GBranchId { get; set; }
        protected String SqlConnectionString { get; set; }
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            using (var context = new ApplicationDbContext())
                SqlConnectionString = context.Database.Connection.ConnectionString;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (!String.IsNullOrEmpty(SqlConnectionString))
                SqlDependency.Start(SqlConnectionString);

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastError = Server.GetLastError();
            if (lastError is HttpRequestValidationException)
            {
                if (Session["UserName"] == null)
                {
                    Response.Redirect("~/Account/Login");
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<html>");
                    sb.Append(@"<body>");
                    sb.Append(@"<div style= 'background-color:red'>");
                    sb.Append(@"<h3 style='color:white'> invalid request </h3>");
                    sb.Append("</div>");
                    sb.Append("</body>");
                    sb.Append("</html>");
                    Response.Write(sb.ToString());
                    Response.End();

                }
                //Response.Redirect("~/Account/LogOff");
            }
        }



        void Session_OnEnd(object sender, EventArgs e)
        {
            int userId = GUserId;
            int branchId = GBranchId;

            Models.LoginLogs sessionObj = uow.Repository<Models.LoginLogs>().FindBy(x => x.UserId == userId && x.BranchId == branchId).LastOrDefault();

            if (sessionObj != null)
            {

                sessionObj.To = DateTime.Now;

                uow.Repository<Models.LoginLogs>().Edit(sessionObj);
                uow.Commit();
            }
            //return new  RedirectResult("/Home/Index");

        }

       
    }
}
