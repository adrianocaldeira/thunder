using System.Collections.Generic;
using Thunder;
using Manager.Models;


namespace Manager.Library
{
    public class Controller : Thunder.Web.Mvc.Controller
    {
        public void SetConnectedUser(User user, IList<Functionality> functionalities)
        {
            Session[HardCode.Session.ConnectedUser] = user;
        }

        protected override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            Logging.Write.Error(filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}