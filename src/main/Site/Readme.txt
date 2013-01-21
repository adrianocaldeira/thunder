1 - Abra o arquivo RouteConfig.cs e adicione a linha new[] { "NomeDoProjeto.Controllers" }, conforme exemplo abaixo:

routes.MapRoute(
    name: "Default",
    url: "{controller}/{action}/{id}",
    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
    namespaces:new[] { "NomeDoProjeto.Controllers" }
);

2 - Para criar, remove, atualizar e dar carga no banco de dados acesso http://localhost:porta/manager/db.

3 - Para acessar a administração do site acesso http://localhost:porta/manager.