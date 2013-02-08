using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Manager.Filters;
using Manager.Library;
using Manager.Models;
using NHibernate.Criterion;
using Newtonsoft.Json;
using Thunder.Data;
using Thunder.Web;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace Manager.Controllers
{
    [Authorized(Order = 2), SessionPerRequest(Order = 1)]
    public class NewsController : ManagerController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.Filter filter)
        {
            return PartialView("List", News.Page(filter.CurrentPage, filter.PageSize,
                new List<Order> { Order.Desc("Date") }));
        }

        [HttpGet]
        public ActionResult New()
        {
            return View("Form", new News());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = News.FindById(id);

            if (model == null)
            {
                return new HttpNotFoundResult();
            }

            return View("Form", model);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            News.Delete(id);

            return new JsonResult();
        }

        [HttpPost]
        public ActionResult Save(News model)
        {
            if (model.IsValid(ModelState))
            {
                model.Sponsor = ConnectedUser;

                if (model.IsNew())
                {
                    News.Create(model);
                }
                else
                {
                    News.Update(model);
                }
                
                AddMessage(Settings.Constants.MessageWithSuccess);

                return new JsonResult { Data = Url.Action("Index", "News") };
            }

            return View(ResultStatus.Attention);
        }
    }
}
