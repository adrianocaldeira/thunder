using System.Web.Mvc;
using Manager.Filters;
using Manager.Library;

namespace Manager.Controllers
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
