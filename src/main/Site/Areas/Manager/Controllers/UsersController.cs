using System.Collections.Generic;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using Thunder.Data;
using Thunder.Web;
using Thunder.Web.Mvc.Filter;
using Thunder.Web.Mvc.Html;
using JsonResult = Thunder.Web.Mvc.JsonResult;
using Site.Filters;
using Site.Library;
using Site.Models;

namespace Site.Areas.Manager.Controllers
{
    [ManagerAuthorized(Order = 2), SessionPerRequest(Order = 1), LayoutInject("Manager")] 
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
            var userDb = Models.User.FindById(id);

            if (userDb == null)
            {
                return new HttpNotFoundResult();
            }

            ViewBag.Profiles = UserProfile.FindByStatus(Status.Active).ToSelectList(x => x.Name, x => x.Id.ToString(),
                userDb.Profile.Id.ToString(),
                new SelectListItem { Selected = true, Text = "Selecione", Value = "" });

            ViewBag.Status = Status.All().ToSelectList(x => x.Name, x => x.Id.ToString(),
                userDb.Status.Id.ToString(),
                new SelectListItem { Selected = true, Text = "Selecione", Value = "" });

            return View("Form", userDb);
        }

        [HttpPost]
        public ActionResult Index(Models.Filter filter)
        {
            return View("_List", Models.User.Page(filter.CurrentPage, filter.PageSize, new List<Order> { Order.Asc("Name") }));
        }

        [HttpPost]
        public ActionResult Save(User user)
        {
            ExcludePropertiesInValidation("Profile.Name", "Profile.Functionalities");

            if (FormIsValid(user))
            {
                user.Password = Models.User.EncriptPassword(user.Password);

                if (user.Id.Equals(0))
                {
                    Models.User.Create(user);
                }
                else
                {
                    Models.User.Update(user);
                }

                AddMessage(HardCode.Constants.MessageWithSuccess);

                return new JsonResult { Data = Url.Action("Index", "Users") };
            }

            return View(ResultStatus.Attention);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Models.User.Delete(id);

            return new JsonResult();
        }

        private bool FormIsValid(User user)
        {
            if (ModelState.IsValid)
            {
                if (Models.User.Exist(user.Id, Restrictions.Eq(Projections.SqlFunction("lower", NHibernateUtil.String, Projections.Property("Login")), user.Login.ToLower())))
                {
                    ModelState.AddModelError("Login", "O login informado já existe.");
                }

                if (!string.IsNullOrEmpty(user.Email) && Models.User.Exist(user.Id, Restrictions.Eq(Projections.SqlFunction("lower", NHibernateUtil.String, Projections.Property("Email")), user.Login.ToLower())))
                {
                    ModelState.AddModelError("Login", "O e-mail informado já existe.");
                }

                return ModelState.IsValid;
            }

            return false;
        }
    }
}