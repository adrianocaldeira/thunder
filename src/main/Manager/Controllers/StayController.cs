using System;
using System.Web.Mvc;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace Manager.Controllers
{
    public class StayController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            Session["Stay"] = DateTime.Now;

            return new JsonResult();
        }
    }
}
