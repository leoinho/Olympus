$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############                  Extrato                ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    $('div.extrato #txtCPF').mask('000.000.000-00');

    // Filtrar
    $("div.extrato #btnFiltrar").click(function (event) {
        event.preventDefault();

        var perfil = $("div.extrato #ddlPerfil").val();
        var ativo = $("div.extrato #ddlAtivo").val();
        var cpf = $("div.extrato #txtCPF").val();

        cpf = cpf.replace(".", "").replace(".", "").replace(".", "").replace("-", "").replace("/", "");

        if (cpf.length > 0 && cpf.length != 11) {
            alerta("Erro", "CPF incorreto");
            return false;
        }

        if (ativo == 0)
            ativo = "false";
        else if (ativo == 1)
            ativo = "true";
        else
            ativo = "";

        window.location.href = "/Loja/Extrato?perfil=" + perfil+"&ativo="+ativo+"&cpf="+cpf;
        return false;
    });
    
    // Exportar
    $("div.extrato #btnExcel").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Loja/ExportarExtrato?" + href;
        return false;
    });

});