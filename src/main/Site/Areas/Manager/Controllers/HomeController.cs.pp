using System.Web.Mvc;
using $rootnamespace$.Filters;
using $rootnamespace$.Library;
using Thunder.Web.Mvc.Filter;

namespace $rootnamespace$.Areas.Manager.Controllers
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
