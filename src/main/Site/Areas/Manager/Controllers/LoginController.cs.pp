using System.Web.Mvc;
using $rootnamespace$.Library;
using Thunder.Data;
using Thunder.Web;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace $rootnamespace$.Areas.Manager.Controllers
{
    public class LoginController : ManagerController
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost, SessionPerRequest]
        public ActionResult Index(string login, string password, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = Models.User.Find(login, password);

                if (user == null)
                {
                    ModelState.AddModelError("login", "Usuário ou senha inválidos.");
                    return View(ResultStatus.Attention);
                }

                ConnectedUser = user;

                return new JsonResult
                {
                    Data = new
                    {
                        Path = string.IsNullOrEmpty(returnUrl) ? Url.Action("Index","Home") : returnUrl
                    }
                };
            }

            return View(ResultStatus.Attention);
        }
    }
}
