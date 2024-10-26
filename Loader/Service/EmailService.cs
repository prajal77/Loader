using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Loader.Repository;
using Loader.Helper;

namespace Loader.Service
{
    public class EmailService
    {
        private GenericUnitOfWork uow;
        public EmailService(IGenericUnitOfWork _uow)
        {
            uow = new GenericUnitOfWork();
        }

        public  string Host;

        public  int Port;
        public  string FromEmail;

        public  string FromPassword;

        #region Constructors
        public EmailService()
        {
            var emailHost = uow.Repository<Models.ParamValue>().FindBy(x => x.PId == (int)EnumValue.Parameter.SMTP).FirstOrDefault();
            if(emailHost!=null)
            {
                Host = emailHost.PValue;
            }

            var emailPort = uow.Repository<Models.ParamValue>().FindBy(x => x.PId == (int)EnumValue.Parameter.Port).FirstOrDefault();
            if(emailPort!=null)
            {
                Port = Convert.ToInt32(emailPort.PValue);
            }

            var fromEmail = uow.Repository<Models.ParamValue>().FindBy(x => x.PId == (int)EnumValue.Parameter.EmailAddress).FirstOrDefault();
            if(fromEmail!=null)
            {
                FromEmail = fromEmail.PValue;
            }

            var fromPWD = uow.Repository<Models.ParamValue>().FindBy(x => x.PId == (int)EnumValue.Parameter.EmailPassword).FirstOrDefault();
            if(fromPWD!=null)
            {
                FromPassword = fromPWD.PValue;
            }
        }
        #endregion

    }
}