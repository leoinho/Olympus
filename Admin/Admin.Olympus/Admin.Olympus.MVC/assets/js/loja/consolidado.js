$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############                Consolidado              ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    $('div.consolidado #txtCPF').mask('000.000.000-00');

    // Filtrar
    $("div.consolidado #btnFiltrar").click(function (event) {
        event.preventDefault();

        var perfil = $("div.consolidado #ddlPerfil").val();
        var status = $("div.consolidado #ddlStatus").val();
        var cpf = $("div.consolidado #txtCPF").val();
        var pedido = $("div.consolidado #txtPedido").val();

        cpf = cpf.replace(".", "").replace(".", "").replace(".", "").replace("-", "").replace("/", "");

        if (cpf.length > 0 && cpf.length != 11) {
            alerta("Erro", "CPF incorreto");
            return false;
        } else if (pedido !== "" && !$.isNumeric(pedido)) {
            alerta("Erro", "Pedido inválido");
            return false;
        }

        window.location.href = "/Loja/Consolidado?perfil=" + perfil+"&status=" + status+"&pedido="+pedido+"&cpf="+cpf;
        return false;
    });
    
    // Exportar
    $("div.consolidado #btnExcel").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Loja/ExportarConsolidado?" + href;
        return false;
    });

});