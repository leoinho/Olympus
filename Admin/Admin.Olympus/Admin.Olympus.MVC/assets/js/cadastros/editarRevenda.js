$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############       Cadastros - Revenda - Edição      ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    var SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
        },
    spOptions = {
        onKeyPress: function (val, e, field, options) {
            field.mask(SPMaskBehavior.apply({}, arguments), options);
        }
    };

    $('div.revendas #txtCelular').mask(SPMaskBehavior, spOptions);

    // Cancelar edição
    $("div.revendas #btnCancelar").click(function (event) {
        event.preventDefault();
        window.location.href = "/Cadastro/RelatorioRevendas/";
    });

    // Salvar edição
    $("div.revendas #btnSalvar").click(function (event) {
        event.preventDefault();

        var id = $("div.revendas #hddIdRevenda").val();
        var dsRazaoSocial = $("div.revendas #txtRazaoSocial").val();
        var dsEmail = $("div.revendas #txtEmail").val();
        var dsCelular = $("div.revendas #txtCelular").cleanVal();
        var idStatus = $("div.revendas #ddlParticipante").val();
        var btAtivo = $("div.revendas #ckbAtivo:checked").length > 0;
        
        if (dsRazaoSocial == "" || dsEmail == "" || idStatus == "" || typeof idStatus == 'undefined' || idStatus <= 0) {
            alerta("Erro", "Preencha corretamente a Razão Social, E-mail e o Status da revenda.");
            return false;
        }

        var url = "/Cadastro/EditarRevenda";
        $.post(url, { idRevenda: id, razaoSocial: dsRazaoSocial, email: dsEmail, celular: dsCelular, status: idStatus, ativo: btAtivo }, function (response) {
            if (response.id > 0) {
                alerta("Sucesso","Revenda alterada.",true);
            }
            else {
                alerta("Erro",response.dsMensagem);
            }
        });
        return false;
    });
});