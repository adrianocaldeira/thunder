using System;
using System.Collections.Generic;
using System.Net;
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
                #region State
                State.Create(State.Active);
                State.Create(State.Inactive);
                #endregion

                #region Módulos
                Module.Create(new Module{ Name = "Cadastros"});
                Module.Create(new Module
                {
                    Name = "Usuários",
                    Parent = new Module { Id = 1 },
                    Functionalities = new List<Functionality>
                    {
                        new Functionality{Name = "Listar", Action = "Index", Controller = "Users", HttpMethod = String.Join(",", WebRequestMethods.Http.Get, WebRequestMethods.Http.Post), Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Criar", Action = "New", Controller = "Users", HttpMethod = WebRequestMethods.Http.Get , Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Editar", Action = "Edit", Controller = "Users", HttpMethod = WebRequestMethods.Http.Get , Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Salvar", Action = "Save", Controller = "Users", HttpMethod = WebRequestMethods.Http.Post, Created = DateTime.Now, Updated = DateTime.Now }
                    }
                });
                #endregion

                #region User
                Models.User.Create(new User
                {
                    Login = "adm",
                    Name = "Administrator",
                    Password = Models.User.EncriptPassword("123"),
                    Functionalities = new List<Functionality> { new Functionality { Id = 1 }, new Functionality { Id = 2 }, new Functionality { Id = 3 }, new Functionality { Id = 4 } }
                });
                #endregion
                
                return Message("Carga inicial do banco de dados efetuada com sucesso!");
            }
            catch (Exception ex)
            {
                new DataBase(SessionManager.Configuration).Drop().Create();

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
            return View("Index", null, string.Format("FALHA: {0}<br>DETALHE: {1}", exception.Message, exception.InnerException));
        }

        private ActionResult Message(string message)
        {
            return View("Index", null, message);
        }
    }
}