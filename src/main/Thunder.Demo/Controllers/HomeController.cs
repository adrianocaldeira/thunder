using System;
using System.Web.Mvc;
using Thunder.Demo.Models;

namespace Thunder.Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new User{Currency = 12233.12m, Date = DateTime.Now});
        }
    }
}
