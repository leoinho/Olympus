$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############            Cadastros - Edição           ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    // Cancelar edição
    $("div.statusRevenda #btnCancelar").click(function (event) {
        event.preventDefault();
        history.back(1);
    });

    // Salvar edição
    $("div.statusRevenda #btnSalvar").click(function (event) {
        event.preventDefault();

        var id = $("div.statusRevenda #hddIdRevenda").val();
        var idStatus = $("div.statusRevenda #ddlParticipante").val();

        if (idStatus == "" || typeof idStatus == 'undefined' || idStatus <= 0) {
            alerta("Erro", "Preencha corretamente o status da revenda.");
            return false;
        }

        var url = "/Cadastro/EditarStatusRevenda";
        $.post(url, { idRevenda: id, status: idStatus }, function (response) {
            if (response.id > 0) {
                alerta("Sucesso","Status da revenda alterado.",true);
            }
            else {
                alerta("Erro",response.dsMensagem);
            }
        });
        return false;
    });
});