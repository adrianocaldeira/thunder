using System.Web.Mvc;
using $rootnamespace$.Library;
using $rootnamespace$.Models;
using Thunder.Data;

namespace $rootnamespace$.Controllers
{
    public class MenuController : ManagerController
    {
        [SessionPerRequest]
        public ActionResult Index()
        {
            return PartialView("Index", Module.FindByUser(Models.User.FindById(ConnectedUser.Id)));
        }

    }
}
