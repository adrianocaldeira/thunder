using System.Web.Mvc;
using Site.Filters;
using Site.Library;
using Thunder.Web.Mvc.Filter;

namespace Site.Areas.Manager.Controllers
{
    [ManagerAuthorized, LayoutInject("Manager")] 
    public class HomeController : ManagerController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
