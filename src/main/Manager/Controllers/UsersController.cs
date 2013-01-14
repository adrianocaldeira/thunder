using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Manager.Library;
using NHibernate;
using NHibernate.Criterion;
using Thunder.Data;
using Thunder.Web;
using Manager.Filters;
using Manager.Models;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace Manager.Controllers
{
    [Authorized]
    public class UsersController : Library.Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpGet, SessionPerRequest]
        public ActionResult New()
        {
            return View("Form", new User());
        }

        [HttpGet, SessionPerRequest]
        public ActionResult Edit(int id)
        {
            var userDb = Models.User.FindById(id);

            if (userDb == null)
            {
                return new HttpNotFoundResult();
            }

            return View("Form", userDb);
        }

        [HttpPost, SessionPerRequest]
        public ActionResult Index(Models.Filter filter)
        {
            return View("_List", Models.User.Page(filter.CurrentPage, filter.PageSize, new List<Order> {Order.Asc("Name")}));
        }

        [HttpPost, SessionPerRequest]
        public ActionResult Save(User user)
        {
            if (FormIsValid(user))
            {
                if (user.Id.Equals(0))
                {
                    Models.User.Create(user);
                }
                else
                {
                    //user.Functionalities = user.Functionalities.Select(
                    //    functionality => Functionality.FindById(functionality.Id)
                    //    ).ToList();
                    Models.User.Update(user);
                }

                AddMessage(HardCode.Constants.MessageWithSuccess);

                return new JsonResult {Data = Url.Action("Index", "Users")};
            }

            return View(ResultStatus.Attention);
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