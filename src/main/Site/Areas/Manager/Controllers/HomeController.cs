using System.Web.Mvc;
using Site.Filters;
using Site.Library;

namespace Site.Areas.Manager.Controllers
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
