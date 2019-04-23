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

    //$("#txtDtNascimento").attr('readonly', 'readonly');
    //$("#txtDtNascimento").datepicker({
    //    changeYear: true,
    //    changeMonth: true,
    //    yearRange: '1900:',
    //    showButtonPanel: true
    //});
    //$('#txtDtNascimento').mask('00/00/0000');
    //$('#txtDtNascimento').on('blur', function (event) {
    //    event.preventDefault();
    //    validaData($(this), $(this).val())
    //});

    //setInterval(function () {
    //    $('.ui-datepicker select').selectBoxIt();
    //}, 10);

    $('#txtDtNascimento').mask('00/00/0000');
    $('#txtCPF').mask('000.000.000-00');

    var SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
	spOptions = {
	    onKeyPress: function (val, e, field, options) {
	        field.mask(SPMaskBehavior.apply({}, arguments), options);
	    }
	};

    $('#txtCelular').mask(SPMaskBehavior, spOptions);

    // Cancelar edição
    $("div.cadastros #btnCancelar").click(function (event) {
        event.preventDefault();
        history.back(1);
    });

    // Salvar edição
    $("div.cadastros #btnSalvar").click(function (event) {
        event.preventDefault();

        var id = $("div.cadastros #hddIdUsuario").val();
        var perfil = $("div.cadastros #hddIdPerfil").val();
        var nome = $("div.cadastros #txtNome").val();
        var nasc = $("div.cadastros #txtDtNascimento").val();
        var email = $("div.cadastros #txtEmail").val();
        var celular = $("div.cadastros #txtCelular").cleanVal();

        if (perfil == 7 || perfil == 9)
        {
            var estadoCivil = $("div.cadastros [name=radEstadoCivil]:checked").attr("id");
            var filhos = $("div.cadastros [name=radFilhos]:checked").attr("id");
            var quantos = $("div.cadastros #txtQuantosFilhos").val();
            var timeFutebol = $("div.cadastros #txtTimeFutebol").val();
            var gostoMusical = $("div.cadastros #txtGostoMusical").val();
            var numeroCalcado = $("div.cadastros #ddlNumeroCalcado").val();
            var tamanhoCamiseta = $("div.cadastros #ddlTamanhoCamiseta").val();
            var cpf = $("div.cadastros #txtCPF").val();
            cpf = cpf.replace(".", "").replace(".", "").replace("-", "")

            if (cpf === "") {
                alerta("Erro", "Preencha corretamente o CPF.");
                return false;
            }
        }
        else 
        {
            var estadoCivil = "";
            var filhos = "";
            var quantos = 0;
            var timeFutebol = "";
            var gostoMusical = "";
            var numeroCalcado = "";
            var tamanhoCamiseta = "";
            var cpf = "";
        }

        var filtro = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;

        if (nome === "" || nasc === "" || email === "") {
            alerta("Erro", "Preencha corretamente o Nome, Data de Nascimento e E-mail.");
            return false;
        } else if (!validaDataNascimento(nasc)) {
            alerta("Erro", "Preencha uma data de nascimento válida, que deve estar entre 01/01/1900 e 31/12/2014.");
            return false;
        } else if (!filtro.test(email)) {
            alerta("Erro","Preencha um email válido.");
            return false;
        } 

        if (typeof estadoCivil == 'undefined')
            estadoCivil = "";

        if (typeof filhos == 'undefined')
            filhos = "";
        else if (filhos == 'nao') {
            filhos = 'n';
            quantos = 0;
        } else if (filhos == 'sim')
            filhos = 's';

        if (quantos === "" || quantos < 0) {
            quantos = 0;
        }

        var url = "/Cadastro/EditarUsuario";
        $.post(url, { idUsuario: id, idPerfil: perfil, dsCPF:cpf, dsNome: nome, dtNascimento: nasc, dsEmail: email, dsCelular: celular, dsEstadoCivil: estadoCivil, dsPossuiFilhos: filhos, inQuantidadeFilhos: quantos, dsTimeFutebol: timeFutebol, dsGostoMusical: gostoMusical, dsNumeroCalcado: numeroCalcado, dsTamanhoCamiseta: tamanhoCamiseta }, function (response) {
            if (response.id > 0) {
                alerta("Sucesso","Cadastro alterado.",true);
            }
            else {
                alerta("Erro",response.dsMensagem);
            }
        });
        return false;
    });
});