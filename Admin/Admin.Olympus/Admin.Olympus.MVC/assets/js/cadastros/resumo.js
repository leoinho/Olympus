$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############            Cadastros - Resumo           ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    // Filtrar
    $("div.resumoCadastros #btnFiltrar").click(function (event) {
        event.preventDefault();

        var gme = $("div.resumoCadastros #ddlGME").val();
        var faixa = $("div.resumoCadastros #ddlFaixa").val();
        var revenda = $("div.resumoCadastros #ddlRevenda").val();

        window.location.href = "/Cadastro/Resumo?gme="+gme+"&faixa="+faixa+"&revenda="+revenda;
        return false;
    });
    
    // Exportar
    $("div.resumoCadastros #btnExcelCadastrosPerfil").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Cadastro/ExportarResumo?tipo=1&" + href;
        return false;
    });

    $("div.resumoCadastros #btnExcelCadastrosSituacao").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Cadastro/ExportarResumo?tipo=2&" + href;
        return false;
    });

    $("div.resumoCadastros #btnExcelRevendasSituacao").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Cadastro/ExportarResumo?tipo=3&" + href;
        return false;
    });

    $("div.resumoCadastros #btnExcelRevendasFaixa").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Cadastro/ExportarResumo?tipo=4&" + href;
        return false;
    });

    $("div.resumoCadastros #btnExcelEquipeCargo").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Cadastro/ExportarResumo?tipo=5&" + href;
        return false;
    });

    $("div.resumoCadastros #btnExcelEquipeSituacao").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Cadastro/ExportarResumo?tipo=6&" + href;
        return false;
    });

    $("div.resumoCadastros #btnExcelRevendasSituacaoDetalhado").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Cadastro/ExportarRevendasSituacaoDetalhado?" + href;
        return false;
    });
    
    // Buscar Revenda
    $("div.resumoCadastros #ddlGME").on('change', function () {
        var idGME = $(this).val();
        var idFaixa = $("div.resumoCadastros #ddlFaixa").val();

        var revendas = $("div.resumoCadastros #ddlRevenda");
        revendas.empty();
        revendas.append($("<option></option>").attr("value", 0).text("--"));

        var url = "/Help/BuscarRevendas";
        $.post(url, { gme: idGME, faixa: idFaixa }, function (response) {
            if (response.id > 0) {
                $.each(response.revendas, function (value, key) {
                    revendas.append($("<option></option>").attr("value", key.idRevenda).text(key.dsRazaoSocial));
                });
            }
            else {
                alerta("Erro", response.dsMensagem);
            }
        });

        revendas.val(0);
    });

    $("div.resumoCadastros #ddlFaixa").on('change', function () {
        var idGME = $("div.resumoCadastros #ddlGME").val();
        var idFaixa = $(this).val();

        var revendas = $("div.resumoCadastros #ddlRevenda");
        revendas.empty();
        revendas.append($("<option></option>").attr("value", 0).text("--"));

        var url = "/Help/BuscarRevendas";
        $.post(url, { gme: idGME, faixa: idFaixa }, function (response) {
            if (response.id > 0) {
                $.each(response.revendas, function (value, key) {
                    revendas.append($("<option></option>").attr("value", key.idRevenda).text(key.dsRazaoSocial));
                });
            }
            else {
                alerta("Erro", response.dsMensagem);
            }
        });

        revendas.val(0);
    });
    
});