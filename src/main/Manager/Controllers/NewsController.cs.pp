using System.Collections.Generic;
using System.Web.Mvc;
using NHibernate.Criterion;
using Thunder.Data;
using Thunder.Web;
using $rootnamespace$.Filters;
using $rootnamespace$.Library;
using $rootnamespace$.Models;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace $rootnamespace$.Controllers
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
                model.Content = model.Content.MoveImages(Url.Action("Viewer", "Files"));

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
