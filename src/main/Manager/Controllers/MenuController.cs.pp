using System.Web.Mvc;
using Thunder.Data;
using $rootnamespace$.Library;
using $rootnamespace$.Models;

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
