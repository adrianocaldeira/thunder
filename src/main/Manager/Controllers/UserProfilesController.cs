using System.Collections.Generic;
using System.Web.Mvc;
using Manager.Filters;
using Manager.Library;
using Manager.Models;
using NHibernate.Criterion;
using Thunder.Data;
using Thunder.Web;
using Thunder.Web.Mvc.Html;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace Manager.Controllers
{
    [Authorized(Order = 2), SessionPerRequest(Order = 1)] 
    public class UserProfilesController : ManagerController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public PartialViewResult Index(Models.Filter filter)
        {
            return PartialView("List", UserProfile.Page(filter.CurrentPage, filter.PageSize, 
                new List<Order> { Order.Asc("Name") }));
        }

        [HttpGet]
        public ActionResult New()
        {
            ViewBag.Status = Status.All().ToSelectList(x => x.Name, x => x.Id.ToString(),
                new SelectListItem { Selected = true, Text = "Selecione", Value = "" });

            return View("Form", new UserProfile());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = UserProfile.FindById(id);

            if (model == null)
            {
                return new HttpNotFoundResult();
            }

            ViewBag.Status = Status.All().ToSelectList(x => x.Name, x => x.Id.ToString(),
                model.Status.Id.ToString(),
                new SelectListItem { Selected = true, Text = "Selecione", Value = "" });

            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Save(UserProfile model)
        {
            if(model.IsValid(ModelState))
            {
                if (model.Id.Equals(0))
                {
                    UserProfile.Create(model);
                }
                else
                {
                    UserProfile.Update(model);
                }

                AddMessage(Settings.Constants.MessageWithSuccess);

                return new JsonResult { Data = Url.Action("Index") };
            }

            return View(ResultStatus.Attention);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            UserProfile.Delete(id);

            return new JsonResult();
        }
    }
}
