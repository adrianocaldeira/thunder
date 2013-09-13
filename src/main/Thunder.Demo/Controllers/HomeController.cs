using System;
using System.Web.Mvc;
using Thunder.Demo.Models;

namespace Thunder.Demo.Controllers
{
    public class HomeController : Web.Mvc.Controller
    {
        public ActionResult Index()
        {
            SetNotify(new Notify(NotifyType.Danger, "Notificação por ViewData do tipo Danger"));
            return View(new User{Currency = 12233.12m, Date = DateTime.Now});
        }
    }
}
