using System.Collections.Generic;
using System.Web.Mvc;
using Manager.Filters;
using Manager.Library;
using Manager.Models;
using NHibernate;
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
        public ActionResult Index(Models.Filter filter)
        {
            return View("_List", UserProfile.Page(filter.CurrentPage, filter.PageSize, new List<Order> { Order.Asc("Name") }));
        }

        [HttpGet]
        public ActionResult New()
        {
            ViewBag.States = State.All().ToSelectList(x => x.Name, x => x.Id.ToString(),
                new SelectListItem { Selected = true, Text = "Selecione", Value = "" });

            return View("Form", new UserProfile());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var userProdileDb = UserProfile.FindById(id);

            if (userProdileDb == null)
            {
                return new HttpNotFoundResult();
            }

            ViewBag.States = State.All().ToSelectList(x => x.Name, x => x.Id.ToString(),
                userProdileDb.State.Id.ToString(),
                new SelectListItem { Selected = true, Text = "Selecione", Value = "" });

            return View("Form", userProdileDb);
        }

        [HttpPost]
        public ActionResult Save(UserProfile profile)
        {
            if (FormIsValid(profile))
            {
                if (profile.Id.Equals(0))
                {
                    UserProfile.Create(profile);
                }
                else
                {
                    UserProfile.Update(profile);
                }

                AddMessage(HardCode.Constants.MessageWithSuccess);

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

        private bool FormIsValid(UserProfile profile)
        {
            if (ModelState.IsValid)
            {
                if (UserProfile.Exist(profile.Id, Restrictions.Eq(Projections.SqlFunction("lower", NHibernateUtil.String, Projections.Property("Name")), profile.Name.ToLower())))
                {
                    ModelState.AddModelError("Name", "O nome informado já existe.");
                }

                return ModelState.IsValid;
            }

            return false;
        }
    }
}
