$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############       Cadastros - Revenda - Cadastro    ############
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
    
    $('div.revendas #txtCNPJ').mask('00.000.000/0000-00');

    // Cancelar edição
    $("div.revendas #btnCancelar").click(function (event) {
        event.preventDefault();
        window.location.href = "/Cadastro/RelatorioRevendas/";
    });

    // Salvar edição
    $("div.revendas #btnSalvar").click(function (event) {
        event.preventDefault();

        var gme = $("div.revendas #ddlGME").val();
        var gve = $("div.revendas #ddlGVE").val();
        var consultor = $("div.revendas #ddlConsultor").val();
        var faixa = $("div.revendas #ddlFaixa").val();
        var tipo = $("div.revendas #ddlTipo").val();
        var r3 = $("div.revendas #txtR3").val();
        var cnpj = $("div.revendas #txtCNPJ").cleanVal();
        var razaoSocial = $("div.revendas #txtRazaoSocial").val();
        var email = $("div.revendas #txtEmail").val();
        var celular = $("div.revendas #txtCelular").cleanVal();
        var status = $("div.revendas #ddlParticipante").val();
        var ativo = $("div.revendas #ckbAtivo:checked").length > 0;
        
        if (gme == "" || gme == 0 ||
            gve == "" || gve == 0 ||
            consultor == "" ||
            faixa == "" || faixa == 0 ||
            tipo == "" || tipo == 0 ||
            r3 == "" ||
            cnpj == "" ||
            razaoSocial == "" ||
            email == "" ||
            status == "" || typeof status == 'undefined' || status <= 0 ||
            ativo === "" || typeof ativo == 'undefined')
        {
            alerta("Erro", "Preencha corretamente o GME, GVE, Consultor, Faixa, Tipo, CNPJ, Razão Social, E-mail e o Status da revenda.");
            return false;
        }

        var participante = false;
        var excluido = false;

        if (status == 1) // Não Participante
        {
            excluido = false;
            participante = false;
        }
        else if (status == 2) // Participante
        {
            excluido = false;
            participante = true;
        }
        else if (status == 3) // Excluída
        {
            excluido = true;
            participante = false;
        }

        var url = "/Cadastro/CadastrarRevenda";
        $.post(url, { idRevenda: 0, idFaixa: faixa, dsFaixa: '', idGME: gme, dsGME: '', idGVE: gve, dsGVE: '', idUsuarioCCE: consultor, idTipo: tipo, dsTipo: '', dsCodigo: r3, dsRazaoSocial: razaoSocial, dsR3: r3, dsCNPJ: cnpj, btParticipante: participante, btExcluida: excluido, btAtivo: ativo, dsCelular: celular, dsEmail: email, dsCCE: '' }, function (response) {
            if (response.id > 0) {
                alerta("Sucesso","Revenda inserida.",false,'/Cadastro/AlterarRevenda?revenda='+response.id);
            }
            else {
                alerta("Erro",response.dsMensagem);
            }
        });
        return false;
    });
});