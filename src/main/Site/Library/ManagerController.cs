using Site.Models;
using Thunder;

namespace Site.Library
{
    public class ManagerController : Thunder.Web.Mvc.Controller
    {
        public User ConnectedUser
        {
            get { return Session[HardCode.Session.ConnectedUser] as User; }
            set { Session[HardCode.Session.ConnectedUser] = value; }
        }

        protected override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            Logging.Write.Error(filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}