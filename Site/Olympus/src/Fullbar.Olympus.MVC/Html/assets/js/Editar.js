

$(function () {

    if ($('input[name="nacionalidade"]').val() == 1) {

        $('input[name="NacionalidadeBrasileira"]').prop("checked", true);
    }
    else {

        $('input[name="NacionalidadeOutros"]').prop("checked", true);
    }


    //$.get('/Home/BuscaCNPJ', { cnpj: $('#CNPJ').val() }, function (resultado) {

    //    if (resultado.dsCNPJ == '') {

    //        alert(resultado.dsMensagem);
    //    }
    //    else {

    //        $('#NomeInstituicao').val(resultado.dsNome);
    //        $('#Cep').val(resultado.dsCep);
    //        $('#UF').val(resultado.dsUF);
    //        $('#Bairro').val(resultado.dsBairro);
    //        $('#Endereco').val(resultado.dsEndereco);
    //        $('#Cidade').val(resultado.dsCidade);
    //        $('#Numero').val(resultado.dsNumero);
    //        $('#Complemento').val(resultado.dsComplemento);
    //    }
    //});


    var SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
        spOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(SPMaskBehavior.apply({}, arguments), options);
            }
        };

    $('#Telefone').mask(SPMaskBehavior, spOptions);
    $('#CPF').mask('000.000.000-00', { reverse: true });
    $('#CNPJ').mask('00.000.000/0000-00', { reverse: true });

    if ($('#erro').val() != '') {

        alert($('#erro').val());
    }

    if ($('#sucesso').val() != '') {

        alert($('#sucesso').val());
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
            $('input[name="NumeroEstado"]').val($('#hdNumeroEstado').val());
        }
        else {

            $('#LicencaMedicaEUANao').attr("checked", true);
            $('#LicencaMedicaEUA').val(2);
            $('#NumeroEstado').hide();
            $('input[name="NumeroEstado"]').val('');
        }
    }



    $('input[name="NacionalidadeBrasileira"]').on("click", function () {

        $('#brasileria').show();

        $('input[name="NacionalidadeOutros"]').prop("checked", false)
        $('input[name="nacionalidade"]').val(1);

    });

    $('input[name="NacionalidadeOutros"]').on("click", function () {

        $('#brasileria').hide();

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



    /*=============================*/
    /*======== BUSCA CNPJ =========*/

    $('#CNPJ').blur(function () {


            $.get('/Editar/BuscaCNPJ', { cnpj: $('#CNPJ').val() }, function (resultado) {

                if (resultado.dsCNPJ == '') {

                    $('#dsNomeFantasia').val('');
                    $('#dsCep').val('');
                    $('#dsUF').val('');
                    $('#dsBairro').val('');
                    $('#dsEndereco').val('');
                    $('#dsCidade').val('');
                    $('#dsNumero').val('');
                    $('#dsComplemento').val('');

                    alert(resultado.dsMensagem);
                }
                else {

                    $('#dsNomeFantasia').val(resultado.dsNome);
                    $('#Cep').val(resultado.dsCep);
                    $('#UF').val(resultado.dsUF);
                    $('#Bairro').val(resultado.dsBairro);
                    $('#Endereco').val(resultado.dsEndereco);
                    $('#Cidade').val(resultado.dsCidade);
                    $('#Numero').val(resultado.dsNumero);
                    $('#Complemento').val(resultado.dsComplemento);
                }
            });
        
    });

});