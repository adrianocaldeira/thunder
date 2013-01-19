using System.Web.Mvc;
using Site.Library;
using Site.Models;
using Thunder.Data;

namespace Site.Areas.Manager.Controllers
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
