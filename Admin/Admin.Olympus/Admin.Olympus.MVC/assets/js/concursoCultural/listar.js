$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############            Concurso Cultural            ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    $('div.concursoCultural #txtLogin').mask('00000000000999');

    // Filtrar
    $("div.concursoCultural #btnFiltrar").click(function (event) {
        event.preventDefault();

        var perfil = $("div.concursoCultural #ddlPerfil").val();
        var gme = $("div.concursoCultural #ddlGME").val();
        var cpf = $("div.concursoCultural #txtLogin").val();

        cpf = cpf.replace(".", "").replace(".", "").replace(".", "").replace("-", "").replace("/", "");

        if (cpf.length > 1 && (cpf.length < 11 || cpf.length > 14 || cpf.length == 12 || cpf.length == 13)) {
            alerta("Erro", "Login (CPF/CNPJ) incorreto");
            return false;
        }

        window.location.href = "/ConcursoCultural/Relatorio?perfil=" + perfil + "&gme=" + gme + "&login=" + cpf;
        return false;
    });

    // Exportar
    $("div.concursoCultural #btnExcel").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/ConcursoCultural/ExportarRelatorio?" + href;
        return false;
    });
});