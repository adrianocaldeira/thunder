using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Thunder.Data;
using $rootnamespace$.Models;

namespace $rootnamespace$.Controllers
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
                Status.Create(Status.Active);
                Status.Create(Status.Inactive);
                #endregion

                #region Módulos
                Module.Create(new Module{ Name = "Cadastros"});
                Module.Create(new Module
                {
                    Name = "Perfis de Usuário",
                    Parent = new Module { Id = 1 },
                    Order = 0,
                    Functionalities = new List<Functionality>
                    {
                        new Functionality{Name = "Listar", Action = "Index", Controller = "UserProfiles", HttpMethod = String.Join(",", WebRequestMethods.Http.Get, WebRequestMethods.Http.Post), Created = DateTime.Now, Updated = DateTime.Now, Default = true},
                        new Functionality{Name = "Criar", Action = "New", Controller = "UserProfiles", HttpMethod = WebRequestMethods.Http.Get , Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Editar", Action = "Edit", Controller = "UserProfiles", HttpMethod = WebRequestMethods.Http.Get , Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Excluir", Action = "Delete", Controller = "UserProfiles", HttpMethod = HttpMethod.Delete.Method , Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Salvar", Action = "Save", Controller = "UserProfiles", HttpMethod = WebRequestMethods.Http.Post, Created = DateTime.Now, Updated = DateTime.Now }
                    }
                });
                Module.Create(new Module
                {
                    Name = "Usuários",
                    Parent = new Module { Id = 1 },
                    Order = 1,
                    Functionalities = new List<Functionality>
                    {
                        new Functionality{Name = "Listar", Action = "Index", Controller = "Users", HttpMethod = String.Join(",", WebRequestMethods.Http.Get, WebRequestMethods.Http.Post), Created = DateTime.Now, Updated = DateTime.Now, Default = true},
                        new Functionality{Name = "Criar", Action = "New", Controller = "Users", HttpMethod = WebRequestMethods.Http.Get , Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Editar", Action = "Edit", Controller = "Users", HttpMethod = WebRequestMethods.Http.Get , Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Excluir", Action = "Delete", Controller = "Users", HttpMethod = HttpMethod.Delete.Method , Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Salvar", Action = "Save", Controller = "Users", HttpMethod = WebRequestMethods.Http.Post, Created = DateTime.Now, Updated = DateTime.Now }
                    }
                });
                Module.Create(new Module
                {
                    Name = "Notícias",
                    Parent = new Module { Id = 1 },
                    Order = 2,
                    Functionalities = new List<Functionality>
                    {
                        new Functionality{Name = "Listar", Action = "Index", Controller = "News", HttpMethod = String.Join(",", WebRequestMethods.Http.Get, WebRequestMethods.Http.Post), Created = DateTime.Now, Updated = DateTime.Now, Default = true},
                        new Functionality{Name = "Criar", Action = "New", Controller = "News", HttpMethod = WebRequestMethods.Http.Get , Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Editar", Action = "Edit", Controller = "News", HttpMethod = WebRequestMethods.Http.Get , Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Excluir", Action = "Delete", Controller = "News", HttpMethod = HttpMethod.Delete.Method , Created = DateTime.Now, Updated = DateTime.Now},
                        new Functionality{Name = "Salvar", Action = "Save", Controller = "News", HttpMethod = WebRequestMethods.Http.Post, Created = DateTime.Now, Updated = DateTime.Now }
                    }
                });
                #endregion

                #region Perfil de Usuário
                UserProfile.Create(new UserProfile
                    {
                        Name = "Administrador do Sistema",
                        Functionalities =
                            new List<Functionality>
                                {
                                    new Functionality {Id = 1},
                                    new Functionality {Id = 2},
                                    new Functionality {Id = 3},
                                    new Functionality {Id = 4},
                                    new Functionality {Id = 5},
                                    new Functionality {Id = 6},
                                    new Functionality {Id = 7},
                                    new Functionality {Id = 8},
                                    new Functionality {Id = 9},
                                    new Functionality {Id = 10},
                                    new Functionality {Id = 11},
                                    new Functionality {Id = 12},
                                    new Functionality {Id = 13},
                                    new Functionality {Id = 14},
                                    new Functionality {Id = 15}
                                }
                    });
                #endregion

                #region User
                Models.User.Create(new User
                {
                    Login = "adm",
                    Name = "Administrator",
                    Password = Models.User.EncriptPassword("123"),
                    Profile = new UserProfile{Id = 1}
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