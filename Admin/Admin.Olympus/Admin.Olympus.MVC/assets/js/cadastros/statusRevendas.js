$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############          Cadastros - Relatórios         ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    $('div.statusRevendas #txtCNPJ').mask('00.000.000/0000-00');

    // Filtrar
    $("div.statusRevendas #btnFiltrar").click(function (event) {
        event.preventDefault();

        var participante = $("div.statusRevendas #ddlParticipante").val();
        var gme = $("div.statusRevendas #ddlGME").val();
        var faixa = $("div.statusRevendas #ddlFaixa").val();
        var cnpj = $("div.statusRevendas #txtCNPJ").val();

        cnpj = cnpj.replace(".", "").replace(".", "").replace(".", "").replace("-", "").replace("/", "");

        if (cnpj.length > 1 && cnpj.length != 14) {
            alerta("Erro", "CNPJ incorreto");
            return false;
        }

        window.location.href = "/Cadastro/RelatorioStatusRevendas?gme=" + gme + "&faixa=" + faixa + "&participante=" + participante + "&cnpj=" + cnpj;
        return false;
    });
    
    // Exportar
    $("div.statusRevendas #btnExcel").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Cadastro/ExportarStatusRevendas?" + href;
        return false;
    });
});