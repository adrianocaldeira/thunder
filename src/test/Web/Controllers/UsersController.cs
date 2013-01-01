using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thunder.Data;
using Thunder.NHibernate;

namespace Web.Controllers
{
    [SessionPerRequest]
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        public ActionResult Index()
        {
            Models.User.FindById(1);
            return View();
        }
    }
}
