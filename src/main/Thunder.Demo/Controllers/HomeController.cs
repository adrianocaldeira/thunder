using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Thunder.Collections;
using Thunder.Demo.Models;

namespace Thunder.Demo.Controllers
{
    public class HomeController : Web.Mvc.Controller
    {
        public ActionResult Index()
        {
            SetNotify(new Notify(NotifyType.Danger, "Notificação por ViewData do tipo Danger"));

            ViewData["Users"] = new Paging<User>(new List<User> { new User(), new User(), new User(), new User() 
            , new User() , new User() , new User() , new User() , new User() , new User() , new User() 
            , new User() , new User() , new User() , new User() , new User() , new User() }, 0, 2);

            return View(new User{Currency = 12233.12m, Date = DateTime.Now});
        }
    }
}
