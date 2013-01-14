using System.Web.Mvc;
using Manager.Filters;

namespace Manager.Controllers
{
    [Authorized]
    public class HomeController : Library.Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
