@model User
@section Scripts{
    <script type="text/javascript">
        $(function () {
            $.manager.users.form();
        });
    </script>
}
@section Title {
Usuário
}

@using (Html.BeginForm("Save", "Users", FormMethod.Post, new { id = "user" }))
{
    @Html.Hidden("Id", Model.Id)    
    using (Html.ContentBox(Model.Id == 0 ? "Novo usuário" : "Alterar usuário"))
    {
    @Helpers.NotificationRequired()
    <p class="column">
        @Html.Label("Profile", "Perfil", true)
        @Html.DropDownList("Profile.Id", (IList<SelectListItem>)ViewBag.Profiles, new { id = "Profile" })
    </p>
    <div class="clear"></div>
    <p class="column">
        @Html.Label("name", "Nome", true)
        @Html.TextBoxFor(x => x.Name, new { id = "name", maxlength = 100, @class = "medium-input-fixed" })
    </p>
    <p class="column">
        @Html.Label("email", "E-mail")
        @Html.TextBoxFor(x => x.Email, new { id = "email", maxlength = 100, @class = "medium-input-fixed" })
    </p>
    <p class="column">
        @Html.Label("login", "Login", true)
        @Html.TextBoxFor(x => x.Login, new { id = "login", maxlength = 20, @class = "small-input-fixed" })
    </p>
    <p class="column">
        @Html.Label("password", "Senha", true)
        @Html.TextBox("Password", (string.IsNullOrEmpty(Model.Password) ? "" : $rootnamespace$.Models.User.DecryptPassword(Model.Password)), new { id = "password", maxlength = 8, @class = "small-input-fixed" })
    </p>
    <p class="column">
        @Html.Label("Status", "Status", true)
        @Html.DropDownList("State.Id", (IList<SelectListItem>)ViewBag.Status, new { id = "Status" })
    </p>    
    <div class="clear">
    </div>
    }
}
@section Actions
{
    <div class="actions">
        <input type="button" value="Salvar" title="Salvar informações" class="button" id="submit" />
        <span id="loading" style="float: right; margin-right: 10px; margin-top: 8px;">Aguarde processando</span>
    </div>
}