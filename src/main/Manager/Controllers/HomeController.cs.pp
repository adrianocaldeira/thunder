using System.Web.Mvc;
using $rootnamespace$.Filters;
using $rootnamespace$.Library;

namespace $rootnamespace$.Controllers
{
    [Authorized]
    public class HomeController : ManagerController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
