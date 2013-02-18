using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Manager.Library;
using Manager.Models.Views;
using Thunder.Web;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace Manager.Controllers
{
    public class FilesController : Controller
    {
        [HttpGet]
        public ActionResult Upload(Upload model)
        {
            return PartialView(model);
        }

        [HttpGet]
        [OutputCache(Duration = 86400, VaryByParam = "directory;file")]
        public ActionResult Viewer(string directory, string file)
        {
            var path = Path.Combine(Settings.FileRepository, HttpUtility.UrlDecode(directory), file);

            if (!System.IO.File.Exists(path))
            {
                return new HttpNotFoundResult();
            }

            return File(path, ContentType.GetFromFile(file));
        }

        [HttpPost]
        public ActionResult Save(Upload model)
        {
            return model.Save(Url);
        }
    }
}
