$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############           Cadastros - Revendas          ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    $('div.revendas #txtCNPJ').mask('00.000.000/0000-00');
    
    // Filtrar
    $("div.revendas #btnFiltrar").click(function (event) {
        event.preventDefault();

        var participante = $("div.revendas #ddlParticipante").val();
        var gme = $("div.revendas #ddlGME").val();
        var faixa = $("div.revendas #ddlFaixa").val();
        var cnpj = $("div.revendas #txtCNPJ").val();

        cnpj = cnpj.replace(".", "").replace(".", "").replace(".", "").replace("-", "").replace("/", "");

        if (cnpj.length > 1 && cnpj.length != 14) {
            alerta("Erro", "CNPJ incorreto");
            return false;
        }

        window.location.href = "/Cadastro/RelatorioRevendas?gme=" + gme + "&faixa=" + faixa + "&participante=" + participante + "&cnpj=" + cnpj;
        return false;
    });
    
    // Exportar
    $("div.revendas #btnExcel").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Cadastro/ExportarRevendas?" + href;
        return false;
    });
});