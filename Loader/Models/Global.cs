using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Reflection;

namespace Loader.Models
{

    public class Global
    {
        private static int fyId = 0;
        public static int CurrentFYID
        {
            get
            {
                if (fyId == 0)
                {
                    Loader.Service.ParameterService param = new Service.ParameterService();
                    fyId = param.GetCurrentFYID();
                }
                return fyId;
            }
            set { fyId = value; }
        }

        public static int UserId { get { return Convert.ToInt32(HttpContext.Current.Session["UserID"]); } }
        public static int BranchId { get { return Convert.ToInt32(HttpContext.Current.Session["BranchId"]); } }
        public static bool IsSuperAdmin { get { Loader.Service.MenuTemplateService param = new Loader.Service.MenuTemplateService(); return param.IsSuperAdmin(); } }
        public static string UserName { get { return Convert.ToString(HttpContext.Current.Session["UserName"]); } }
        public static string CurrentFiscalYear
        {
            get
            {
                Loader.Service.ParameterService param = new Service.ParameterService();
                return param.GetCurrentFiscalYear(CurrentFYID) == "" ? DateTime.Now.ToShortDateString() : param.GetCurrentFiscalYear();

            }

        }
       
        public static Nullable<DateTime> TransactionDate { get { Loader.Service.BranchSetupService param = new Loader.Service.BranchSetupService(); return param.GetTransactionDateOfCurrentBranch(BranchId); } }
    }


}