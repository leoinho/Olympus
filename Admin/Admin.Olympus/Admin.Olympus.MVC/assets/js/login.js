$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############                 Login                   ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    $("#form-login #btnEntrar").click(function () {
        var login = $("#form-login #txtLogin").val();
        var senha = $("#form-login #txtSenha").val();

        if (login === "" || senha === "") {
            alerta("Erro", "Preencha seu login e senha.");
            return false;
        }

        var url = "/Login/Index";
        $.post(url, { Login: login, Senha: senha }, function (data) {
            if (data === false) {
                alerta("Erro","O login ou a senha inseridos \n estão incorretos.");
            }
            else {
                window.location.href = "Dashboard";
            }
        });
        return false;
    });
});