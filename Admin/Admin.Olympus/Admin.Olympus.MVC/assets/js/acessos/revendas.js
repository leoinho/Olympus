$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############           Acessos - Revendas            ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    // Filtrar
    $("div.revendas #btnFiltrar").click(function (event) {
        event.preventDefault();

        var mes = $("div.revendas #ddlMes").val();
        var ano = $("div.revendas #ddlAno").val();
        var gme = $("div.revendas #ddlGME").val();
        var faixa = $("div.revendas #ddlFaixa").val();
        var revenda = $("div.revendas #ddlRevenda").val();

        var de = $("div.revendas #txtDataDe").val();
        var ate = $("div.revendas #txtDataAte").val();

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

        window.location.href = "/Acesso/RelatorioRevendas?mes=" + mes + "&ano=" + ano + "&gme=" + gme + "&faixa=" + faixa + "&revenda=" + revenda + "&de=" + de + "&ate=" + ate;
        return false;
    });
    
    // Exportar
    $("div.revendas #btnExcel").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Acesso/ExportarRevendas?" + href;
        return false;
    });

    // Buscar Revenda
    $("div.revendas #ddlGME").on('change', function () {
        var idGME = $(this).val();
        var idFaixa = $("div.revendas #ddlFaixa").val();

        var revendas = $("div.revendas #ddlRevenda");
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

    $("div.revendas #ddlFaixa").on('change', function () {
        var idGME = $("div.revendas #ddlGME").val();
        var idFaixa = $(this).val();

        var revendas = $("div.revendas #ddlRevenda");
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