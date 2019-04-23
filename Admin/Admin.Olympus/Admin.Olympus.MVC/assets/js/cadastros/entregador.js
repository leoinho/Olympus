$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############          Cadastros - Entregadores       ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    $('div.entregadores #txtCPF').mask('00000000000999');

    // Filtrar
    $("div.entregadores #btnFiltrar").click(function (event) {
        event.preventDefault();

        var ativo = $("div.entregadores #ddlAtivo").val();
        var gme = $("div.entregadores #ddlGME").val();
        var faixa = $("div.entregadores #ddlFaixa").val();
        var revenda = $("div.entregadores #ddlRevenda").val();
        var cpf = $("div.entregadores #txtCPF").val();

        cpf = cpf.replace(".", "").replace(".", "").replace(".", "").replace("-", "").replace("/", "");

        if (cpf.length > 1 && (cpf.length < 11 || cpf.length > 14 || cpf.length == 12 || cpf.length == 13)) {
            alerta("Erro", "Login (CPF/CNPJ) incorreto");
            return false;
        }

        if (ativo == 0)
            ativo = "false";
        else if (ativo == 1)
            ativo = "true";
        else
            ativo = "";

        window.location.href = "/Cadastro/RelatorioEntregador?gme=" + gme + "&faixa=" + faixa + "&revenda=" + revenda + "&ativo=" + ativo + "&cpf=" + cpf;
        return false;
    });

    // Exportar
    $("div.entregadores #btnExcel").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Cadastro/ExportarEntregador?" + href;
        return false;
    });

    // Ativar
    $("div.entregadores .activeUser").click(function (event) {
        event.preventDefault();

        var id = $(this).attr("id");
        id = id.replace("btnActiveUser", "");

        var url = "/Cadastro/AtivarUsuario";
        $.post(url, { idUsuario: id }, function (response) {
            if (response.id > 0) {
                alerta("Sucesso", "Usuário ativado.", true);
            }
            else {
                alerta("Erro", response.dsMensagem);
            }
        });
        return false;
    });

    // Inativar
    $("div.entregadores .inactiveUser").click(function (event) {
        event.preventDefault();

        var id = $(this).attr("id");
        id = id.replace("btnInactiveUser", "");

        var url = "/Cadastro/InativarUsuario";
        $.post(url, { idUsuario: id }, function (response) {
            if (response.id > 0) {
                alerta("Sucesso", "Usuário inativado.", true);
            }
            else {
                alerta("Erro", response.dsMensagem);
            }
        });
        return false;
    });

    // Buscar Revenda
    $("div.entregadores #ddlGME").on('change', function () {
        var idGME = $(this).val();
        var idFaixa = $("div.entregadores #ddlFaixa").val();

        var revendas = $("div.entregadores #ddlRevenda");
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

    $("div.entregadores #ddlFaixa").on('change', function () {
        var idGME = $("div.entregadores #ddlGME").val();
        var idFaixa = $(this).val();

        var revendas = $("div.entregadores #ddlRevenda");
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