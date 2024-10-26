using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.DataAccess
{

    public class CookiesHelper
    {
        public void UpdateCookie(string SessionName, string SessionValue)
        {
            HttpContext.Current.Session[SessionName] = SessionValue;
        }

        public object GetSessionValue(string SessionVariableName)
        {
            return HttpContext.Current.Session[SessionVariableName];
        }
    }


    public class CHGlobalData
    {
        private static CookiesHelper CH = new CookiesHelper();

        public static int UserId
        {
            get
            {
                if(HttpContext.Current.Session["UserId"] != null)
                {
                    string text = HttpContext.Current.Session["UserId"].ToString();
                    if(text.Length == 0)
                    {
                        HttpContext.Current.Response.End();
                        throw new Exception("ERROR! Session Time Expired.");
                    }
                    return Convert.ToInt32(text);
                }
                HttpContext.Current.Response.End();
                throw new Exception("ERROR! Session Time Expired.");
            }
        }

        public static int EmployeeId
        {
            get
            {
                string text = CH.GetSessionValue("EmployeeId").ToString();
                if (text.Length == 0)
                {
                    HttpContext.Current.Response.End();
                    return 0;
                }

                return Convert.ToInt32(text);
            }
        }


    }
}