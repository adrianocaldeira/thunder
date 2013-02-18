using System.Web.Mvc;
using Thunder.Data;
using Thunder.Web;
using $rootnamespace$.Library;
using $rootnamespace$.Models;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace $rootnamespace$.Controllers
{
    public class LoginController : ManagerController
    {
        public ActionResult Index()
        {
            return View("Index", new User());
        }

        [HttpPost]
        [SessionPerRequest]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User model, string returnUrl)
        {
            ExcludePropertiesInValidation("Name");

            if (ModelState.IsValid)
            {
                var user = Models.User.Find(model.Login, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("Login", "Usuário ou senha inválidos.");

                    return View(ResultStatus.Attention);
                }

                ConnectedUser = user;

                return new JsonResult
                {
                    Data = new
                    {
                        Path = string.IsNullOrEmpty(returnUrl) ? Url.Action("Index", "Home") : returnUrl
                    }
                };
            }

            return View(ResultStatus.Attention);
        }
    }
}