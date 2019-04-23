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

    $('div.cadastros #txtCPF').mask('00000000000999');

    // Filtrar
    $("div.cadastros #btnFiltrar").click(function (event) {
        event.preventDefault();

        var perfil = $("div.cadastros #ddlPerfil").val();
        var ativo = $("div.cadastros #ddlAtivo").val();
        var gme = $("div.cadastros #ddlGME").val();
        var faixa = $("div.cadastros #ddlFaixa").val();
        var revenda = $("div.cadastros #ddlRevenda").val();
        var cpf = $("div.cadastros #txtCPF").val();

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

        window.location.href = "/Cadastro/Relatorio?perfil=" + perfil+"&gme="+gme+"&faixa="+faixa+"&revenda="+revenda+"&ativo="+ativo+"&cpf="+cpf;
        return false;
    });
    
    // Exportar
    $("div.cadastros #btnExcel").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Cadastro/ExportarRelatorio?" + href;
        return false;
    });

    // Ativar
    $("div.cadastros .activeUser").click(function (event) {
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
    $("div.cadastros .inactiveUser").click(function (event) {
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
    $("div.cadastros #ddlGME").on('change', function () {
        var idGME = $(this).val();
        var idFaixa = $("div.cadastros #ddlFaixa").val();

        var revendas = $("div.cadastros #ddlRevenda");
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

    $("div.cadastros #ddlFaixa").on('change', function () {
        var idGME = $("div.cadastros #ddlGME").val();
        var idFaixa = $(this).val();

        var revendas = $("div.cadastros #ddlRevenda");
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