using System.Web.Mvc;
using Thunder.Data;
using Thunder.Web;
using JsonResult = Thunder.Web.Mvc.JsonResult;
using Manager.Models;

namespace Manager.Controllers
{
    public class LoginController : Library.Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost, SessionPerRequest]
        public ActionResult Index(string login, string password, string path)
        {
            if (ModelState.IsValid)
            {
                var user = Models.User.Find(login, password);

                if (user == null)
                {
                    ModelState.AddModelError("login", "Usuário ou senha inválidos.");
                    return View(ResultStatus.Attention);
                }

                //SetConnectedUser(user, Functionality.Parents());

                return new JsonResult
                {
                    Data = new
                    {
                        Path = string.IsNullOrEmpty(path) ? Url.Content("~/") : path
                    }
                };
            }

            return View(ResultStatus.Attention);
        }
    }
}
