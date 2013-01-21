using System.Web.Mvc;
using Thunder.Data;
using Site.Library;
using Site.Models;

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
