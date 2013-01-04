using System;
using System.Web.Mvc;
using Manager.Models;
using Thunder.Data;

namespace Manager.Controllers
{
    public class DbController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index", null, "");
        }

        [HttpGet]
        public ActionResult Drop()
        {
            try
            {
                new DataBase(SessionManager.Configuration).Drop();

                return Message("Banco de dados removido com sucesso!");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
           try
            {
                new DataBase(SessionManager.Configuration).Create();

                return Message("Banco de dados criado com sucesso!");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public ActionResult Update()
        {
            try
            {
                new DataBase(SessionManager.Configuration).Update();

                return Message("Banco de dados atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [SessionPerRequest, HttpGet]
        public ActionResult Dump()
        {
            try
            {
                State.Create(State.Active);
                State.Create(State.Inactive);

                return Message("Carga inicial do banco de dados efetuada com sucesso!");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = Error(filterContext.Exception);
            filterContext.ExceptionHandled = true;
        }

        private ActionResult Error(Exception exception)
        {
            return View("Index", null, string.Format("FALHA: {0}", exception.Message));
        }

        private ActionResult Message(string message)
        {
            return View("Index", null, message);
        }
    }
}