$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############                  Detalhado              ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    $('div.detalhado #txtCPF').mask('000.000.000-00');

    // Filtrar
    $("div.detalhado #btnFiltrar").click(function (event) {
        event.preventDefault();

        var perfil = $("div.detalhado #ddlPerfil").val();
        var status = $("div.detalhado #ddlStatus").val();
        var cpf = $("div.detalhado #txtCPF").val();
        var pedido = $("div.detalhado #txtPedido").val();

        cpf = cpf.replace(".", "").replace(".", "").replace(".", "").replace("-", "").replace("/", "");

        if (cpf.length > 0 && cpf.length != 11) {
            alerta("Erro", "CPF incorreto");
            return false;
        } else if (pedido !== "" && !$.isNumeric(pedido)) {
            alerta("Erro", "Pedido inválido");
            return false;
        }

        window.location.href = "/Loja/Detalhado?perfil=" + perfil+"&status=" + status+"&pedido="+pedido+"&cpf="+cpf;
        return false;
    });
    
    // Exportar
    $("div.detalhado #btnExcel").click(function (event) {
        event.preventDefault();

        var href = window.location.href.split("?")[1];
        window.location.href = "/Loja/ExportarDetalhado?" + href;
        return false;
    });

});