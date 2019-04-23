$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############          Duvidas - Relatórios           ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    $('div.duvidas #txtLogin').mask('00000000000999');

    // Filtrar
    $("div.duvidas #btnFiltrar").click(function (event) {
        event.preventDefault();

        var perfil = $("div.duvidas #ddlPerfil").val();
        var mes = $("div.duvidas #ddlMes").val();
        var ano = $("div.duvidas #ddlAno").val();
        var login = $("div.duvidas #txtLogin").val();

        login = login.replace(".", "").replace(".", "").replace(".", "").replace("-", "").replace("/", "");

        if (login.length > 1 && (login.length < 11 || login.length > 14 || login.length == 12 || login.length == 13)) {
            alerta("Erro", "Login (CPF/CNPJ) incorreto");
            return false;
        }
        
        window.location.href = "/FaleConosco/Relatorio?perfil=" + perfil+"&mes="+mes+"&ano="+ano+"&login="+login;
        return false;
    });
    
    // Exportar
    $("div.duvidas #btnExcel").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/FaleConosco/ExportarRelatorio?" + href;
        return false;
    });
});