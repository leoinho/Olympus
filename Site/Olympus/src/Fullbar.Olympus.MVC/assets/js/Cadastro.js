
$(function () {

    $(".esconde-cadastro").hide();

    var SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
   spOptions = {
       onKeyPress: function (val, e, field, options) {
           field.mask(SPMaskBehavior.apply({}, arguments), options);
       }
   };

    $('#Telefone').mask(SPMaskBehavior, spOptions);
    $('#Celular').mask(SPMaskBehavior, spOptions);
    $('#TelefoneEstrangeiro').mask(SPMaskBehavior, spOptions);

    $('#CPF').mask('000.000.000-00', { reverse: true });
    $('#CNPJ').mask('00.000.000/0000-00', { reverse: true });

    $('#Numero').mask("99999999");
    

    if ($('#erroCadastro').val() != '') {

        var titulo = $('#erroTituloCadastro').val();
        var texto = $('#erroCadastro').val();

        $('#erroTituloCadastro').val("");
        $('#erroCadastro').val("");

        alerta(texto,titulo);

    }


    if ($('input[name="OutrosTitulosProfissional"]').val() != '') {

        $('#outros').attr("checked", true);
        $('input[name="TituloProfissionalOutros"]').show();
        $('input[name="TituloProfissionalOutros"]').val($('input[name="OutrosTitulosProfissional"]').val());
    }

    if ($('input[name="hdTituloProfissional"]').val() != '') {

        var valor = $('input[name="hdTituloProfissional"]').val();

        if (valor == 'COREN') $('#coren').attr("checked", true);
        if (valor == 'CRM') $('#crm').attr("checked", true);
        if (valor == 'CREA') $('#crea').attr("checked", true);

    }

    if ($('input[name="hdLicencaMedicaEUA"]').val() != '') {

        var licencaMedica = $('input[name="hdLicencaMedicaEUA"]').val();

        if (licencaMedica == 1) {

            $('#LicencaMedicaEUASim').attr("checked", true);
            $('#LicencaMedicaEUA').val(1);

            $('#NumeroEstado').show();
        }
        else {

            $('#LicencaMedicaEUANao').attr("checked", true);
            $('#LicencaMedicaEUA').val(2);
            $('#NumeroEstado').hide();
        }
    }

    

    if ($('input[name="nacionalidade"]').val() == 1) {        
        $('#brasileria').show();
        $('#estrangeiro').hide();
        $('#CNPJOutros').hide();
        $('#Pais').hide();

        $('input[name="NacionalidadeBrasileira"]').prop("checked", true)
        $('input[name="NacionalidadeOutros"]').prop("checked", false)
        $('input[name="nacionalidade"]').val(1);
    }


    if ($('input[name="nacionalidade"]').val() == 2) {       
        $('#brasileria').hide();
        $('#estrangeiro').show();
        $('#CNPJOutros').hide();
        
        $('#Pais').show();

        $('input[name="NacionalidadeBrasileira"]').prop("checked", false)
        $('input[name="NacionalidadeOutros"]').prop("checked", false)
        $('input[name="CNPJ"]').prop("checked", false)
        $('input[name="nacionalidade"]').val(2);
        
    }

    if (!$('input[name^=Nacionalidade]').is(':checked')) {
        $('.hide-cadastro').hide();        
    }


    $('input[name="NacionalidadeBrasileira"]').on("click", function () {

        $(".resp-errada").removeClass("resp-errada");

        $('.hide-cadastro').show();  
        $('#brasileria').show();
        $('#estrangeiro').hide();
        $('#CNPJOutros').hide();
        $('#CNPJBrasileiro').show();
        $('#Pais').hide();

        $('input[name="NacionalidadeOutros"]').prop("checked", false)
        $('input[name="nacionalidade"]').val(1);

    });

    $('input[name="NacionalidadeOutros"]').on("click", function () {

        $(".resp-errada").removeClass("resp-errada");

        $('.hide-cadastro').show();
        $('#brasileria').hide();
        $('#estrangeiro').show();
        $('#CNPJOutros').hide();
        $('#CNPJBrasileiro').hide();
        $('#Pais').show();

        $('input[name="NacionalidadeBrasileira"]').prop("checked", false)
        $('input[name="nacionalidade"]').val(2);

    });


    $('input[name="TituloProfissional"]').on("click", function () {

        if ($('input[name="TituloProfissional"]').is(":checked")) {

            if ($(this).val() == '') {

                $('input[name="TituloProfissionalOutros"]').show();
            }

            else {

                $('input[name="TituloProfissionalOutros"]').val('');
                $('input[name="TituloProfissionalOutros"]').hide();
            }
        }

    });


    $('#LicencaMedicaEUASim').on("click", function () {

        $('#LicencaMedicaEUANao').attr("checked", false);

        $('#LicencaMedicaEUA').val(1);

        $('input[name="NumeroEstado"]').val('');
        $('#NumeroEstado').show();

    });

    $('#LicencaMedicaEUANao').on("click", function () {


        $('#LicencaMedicaEUA').val(0);
        $('#NumeroEstado').hide();
        $('input[name="NumeroEstado"]').val('');

    });


    $('#chkInformacaoVerdadeira').on('click', function () {

        var isChecado = $('#chkInformacaoVerdadeira').is(':checked');

        if (isChecado) {

            $('#InformacaoVerdadeira').val(true);
        }
        else {
                
            $('#InformacaoVerdadeira').val(false);
        }

    });

    var isInformacaoVerdadeira = $('#InformacaoVerdadeira').val();

    if (isInformacaoVerdadeira == 1) {

        $(".selec-cadastro").trigger("click");

        $('#chkInformacaoVerdadeira').prop('checked', true);
        $('#InformacaoVerdadeira').val(true);
     
    }


    $('#btCadastro').on("click", function () {
        
        $("#carregando").show();


        var form = document.getElementById('formCadastro');
        formData = new FormData(form);

        var url = "/Cadastro/Cadastrar";
        $.ajax({
            type: "POST",
            url: url,
            dataType: "json",
            //contentType: "multipart/form-data",
            data: formData,
            contentType: false,
            processData: false,
            success: function (retorno) {
                $("#carregando").hide();
                if (retorno.btSucesso) {
                    if (retorno.treinamento) {
                        //cadastro(retorno.dsTitulo, retorno.dsMensagem);
                        window.location.href = "Treinamento/Inscricao";
                    }
                    else {
                        window.location.href = "Home";
                    }
                }
                else {
                    if (retorno.dsMensagem != null) {
                        $(".resp-errada").removeClass("resp-errada");
                        var str = retorno.dsMensagem;
                        var ids = str.split("#");

                        for (i = 0; i < ids.length; i++) {
                            $("#" + ids[i]).addClass("resp-errada");
                        }
                    }

                    alerta(retorno.dsTexto, retorno.dsTitulo);
                }
            }
        });
    });

    $("#btnInscrever").on("click", function () {
        window.location.href = "Treinamento/Inscricao";
    });

    $("#btnNaoInscrever").on("click", function () {
        window.location.href = "Home";
    });
    


    /*=============================*/
    /*======== BUSCA CNPJ =========*/

    $('#CNPJ').blur(function () {


        if ($('#CNPJ').val() != '') {

            //$('#NomeInstituicao').val('');
            //$('#Cep').val('');
            //$('#UF').val('');
            //$('#Bairro').val('');
            //$('#Endereco').val('');
            //$('#Cidade').val('');
            //$('#Numero').val('');
            //$('#Complemento').val('');

            $("#carregando").show();

            $.get('/Cadastro/BuscaCNPJ', { cnpj: $('#CNPJ').val() }, function (resultado) {

                $("#carregando").hide();

                if (resultado.dsCNPJ == '') {

                    alerta(resultado.dsMensagem, resultado.dsTitulo);

                    //$('#NomeInstituicao').removeAttr("readonly");
                    //$('#Cep').removeAttr("readonly");
                    //$('#UF').removeAttr("readonly");
                    //$('#Bairro').removeAttr("readonly");
                    //$('#Endereco').removeAttr("readonly");
                    //$('#Cidade').removeAttr("readonly");
                    //$('#Numero').removeAttr("readonly");
                    //$('#Complemento').removeAttr("readonly");

                    $('#NomeInstituicao').focus();
                }
                else {

                    $('#NomeInstituicao').val(resultado.dsNomeFantasia);
                    $('#Cep').val(resultado.dsCep);
                    $('#UF').val(resultado.dsUF);
                    $('#Bairro').val(resultado.dsBairro);
                    $('#Endereco').val(resultado.dsEndereco);
                    $('#Cidade').val(resultado.dsCidade);
                    $('#Numero').val(resultado.dsNumero);
                    $('#Complemento').val(resultado.dsComplemento);

                    //$('#NomeInstituicao').attr("readonly", "readonly");
                    //$('#Cep').attr("readonly", "readonly");
                    //$('#UF').attr("readonly", "readonly");
                    //$('#Bairro').attr("readonly", "readonly");
                    //$('#Endereco').attr("readonly", "readonly");
                    //$('#Cidade').attr("readonly", "readonly");
                    //$('#Numero').attr("readonly", "readonly");
                    //$('#Complemento').attr("readonly", "readonly");
                }

                
            });
        }
    });
});



function Onlychars(e) {
    var tecla = new Number();
    if (window.event) {
        tecla = e.keyCode;
    }
    else if (e.which) {
        tecla = e.which;
    }
    else {
        return true;
    }
    if ((tecla >= "48") && (tecla <= "57")) {
        return false;
    }
}



$("#Cep").blur(function () {

    $("#carregando").show();

    var cep = $(this).val().replace(/[^0-9]/, '');

    if (cep !== "") {
        $.ajax({
            type: "GET",
            url: "http://cep.republicavirtual.com.br/web_cep.php",
            data: { cep: $(this).val(), formato: "json" },
            success: function (data) {
                $("#Endereco").val(data.tipo_logradouro + " " + data.logradouro);
                $("#Bairro").val(data.bairro);
                $("#Cidade").val(data.cidade);
                $("#UF").val(data.uf);
                $("#Numero").focus();

                $("#carregando").hide();
            }
        });

    } else {
        $("#carregando").hide();
    }

});