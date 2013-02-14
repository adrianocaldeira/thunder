using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Manager.Controllers
{
    public class FilesController : Controller
    {
        [HttpGet]
        public ActionResult Upload(IList<string> extensions, string directory = "Files", bool multiple = false)
        {
            ViewBag.Extensions = string.Join(";", extensions);
            ViewBag.Directory = directory;
            ViewBag.Multiple = multiple;

            return View();
        }
    }
}
