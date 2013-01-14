using System.Web.Mvc;

namespace Manager.Controllers
{
    public class LogOffController : Controller
    {
        public ActionResult Index()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}
