using Thunder;
using Manager.Models;


namespace Manager.Library
{
    public class ManagerController : Thunder.Web.Mvc.Controller
    {
        public User ConnectedUser
        {
            get { return Session[Settings.Session.ConnectedUser] as User; }
            set { Session[Settings.Session.ConnectedUser] = value; }
        }

        protected override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            Logging.Write.Error(filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}