$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############             Acessos - Resumo            ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################
    
    $('div.resumoAcessos .txtData').mask('00/00/0000');

    // Filtrar
    $("div.resumoAcessos #btnFiltrar").click(function (event) {
        event.preventDefault();

        var mes = $("div.resumoAcessos #ddlMes").val();
        var ano = $("div.resumoAcessos #ddlAno").val();
        var perfil = $("div.resumoAcessos #ddlPerfil").val();
        var gme = $("div.resumoAcessos #ddlGME").val();
        var faixa = $("div.resumoAcessos #ddlFaixa").val();
        var revenda = $("div.resumoAcessos #ddlRevenda").val();

        var de = $("div.resumoAcessos #txtDataDe").val();
        var ate = $("div.resumoAcessos #txtDataAte").val();

        if (!validaData(de)) {
            alerta("Erro", "Preencha uma data inicial válida.");
            return false;
        } else if (!validaData(ate)) {
            alerta("Erro", "Preencha uma data final válida.");
            return false;
        }

        if (de !== "") {
            var aux = de.split('/');
            de = aux[1] + "%2f" + aux[0] + "%2f" + aux[2];
        }

        if (ate !== "") {
            var aux = ate.split('/');
            ate = aux[1] + "%2f" + aux[0] + "%2f" + aux[2];
        }

        window.location.href = "/Acesso/Resumo?mes=" + mes + "&ano=" + ano + "&perfil=" + perfil + "&gme=" + gme + "&faixa=" + faixa + "&revenda=" + revenda + "&de=" + de + "&ate=" + ate;
        return false;
    });
    
    // Exportar
    $("div.resumoAcessos #btnExcelAcessosPerfil").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Acesso/ExportarResumo?tipo=1&" + href;
        return false;
    });

    $("div.resumoAcessos #btnExcelAcessosGME").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Acesso/ExportarResumo?tipo=2&" + href;
        return false;
    });

    $("div.resumoAcessos #btnExcelAcessosFaixa").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Acesso/ExportarResumo?tipo=3&" + href;
        return false;
    });
    
    $("div.resumoAcessos #btnExcelAcessosEquipeCargo").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Acesso/ExportarResumo?tipo=4&" + href;
        return false;
    });

    $("div.resumoAcessos #btnExcelAcessosMes").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Acesso/ExportarResumo?tipo=5&" + href;
        return false;
    });


    $("div.resumoAcessos #btnExcelAcessosDiaSemana").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Acesso/ExportarResumo?tipo=6&" + href;
        return false;
    });

    $("div.resumoAcessos #btnExcelAcessosDia").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Acesso/ExportarResumo?tipo=7&" + href;
        return false;
    });

    // Buscar Revenda
    $("div.resumoAcessos #ddlGME").on('change', function () {
        var idGME = $(this).val();
        var idFaixa = $("div.resumoAcessos #ddlFaixa").val();

        var revendas = $("div.resumoAcessos #ddlRevenda");
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

    $("div.resumoAcessos #ddlFaixa").on('change', function () {
        var idGME = $("div.resumoAcessos #ddlGME").val();
        var idFaixa = $(this).val();

        var revendas = $("div.resumoAcessos #ddlRevenda");
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