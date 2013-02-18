using System.Collections.Generic;
using System.Web.Mvc;
using NHibernate.Criterion;
using Thunder.Data;
using Thunder.Web;
using Thunder.Web.Mvc.Filter;
using Thunder.Web.Mvc.Html;
using Manager.Filters;
using Manager.Models;
using Manager.Library;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace Manager.Controllers
{
    [Authorized(Order = 2), SessionPerRequest(Order = 1)] 
    public class UsersController : ManagerController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpGet, LayoutInject("Modal")]
        public ActionResult New()
        {
            ViewBag.Profiles = UserProfile.FindByStatus(Status.Active).ToSelectList(x => x.Name, x => x.Id.ToString(),
                new SelectListItem { Selected = true, Text = "Selecione", Value = "" });

            ViewBag.Status = Status.All().ToSelectList(x => x.Name, x => x.Id.ToString(),
                new SelectListItem { Selected = true, Text = "Selecione", Value = "" });

            return View("Form", new User());
        }

        [HttpGet, LayoutInject("Modal")]
        public ActionResult Edit(int id)
        {
            var model = Models.User.FindById(id);

            if (model == null)
            {
                return new HttpNotFoundResult();
            }

            ViewBag.Profiles = UserProfile.FindByStatus(Status.Active).ToSelectList(x => x.Name, x => x.Id.ToString(),
                model.Profile.Id.ToString(),
                new SelectListItem { Selected = true, Text = "Selecione", Value = "" });

            ViewBag.Status = Status.All().ToSelectList(x => x.Name, x => x.Id.ToString(),
                model.Status.Id.ToString(),
                new SelectListItem { Selected = true, Text = "Selecione", Value = "" });

            return View("Form", model);
        }

        [HttpPost]
        public PartialViewResult Index(Models.Filter filter)
        {
            return PartialView("List", Models.User.Page(filter.CurrentPage, filter.PageSize, new List<Order> { Order.Asc("Name") }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(User model)
        {
            ExcludePropertiesInValidation("Profile.Name", "Profile.Functionalities");

            if (model.IsValid(ModelState))
            {
                model.Password = Models.User.EncriptPassword(model.Password);

                if (model.Id.Equals(0))
                {
                    Models.User.Create(model);
                }
                else
                {
                    Models.User.Update(model);
                }

                return new JsonResult ();
            }

            return View(ResultStatus.Attention);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Models.User.Delete(id);

            return new JsonResult();
        }
    }
}