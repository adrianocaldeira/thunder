using System.Web.Mvc;
using Manager.Library;
using Manager.Models;
using Thunder.Data;

namespace Manager.Controllers
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
