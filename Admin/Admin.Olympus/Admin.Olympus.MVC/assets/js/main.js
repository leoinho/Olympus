function alerta(titulo, mensagem, reload, href) {
    $('#myModalLabel').text(titulo);
    $('#myModalTexto').html(mensagem);
    $('#myModal').modal('show');

    if (reload == true) {
        $('#myModal .fechar').on('click', function (event) {
            event.preventDefault();
            window.location.reload();
        });
		
    };

    if (href != undefined) {
        $('#myModal .fechar').on('click', function (event) {
            event.preventDefault();
            window.location.href = href
        });
    }
}

function confirma(titulo, mensagem) {

    $('#myModalConfirmLabel').text(titulo);
    $('#myModalConfirmTexto').html(mensagem);
    $('#myModalConfirm').modal('show');

}



function validaData(valor) {
    var date = valor;
    var ardt = new Array;
    var ExpReg = new RegExp("(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}");
    ardt = date.split("/");
    erro = false;
    if (date == "")
        erro == false;
    else if (((ardt[1] == 4) || (ardt[1] == 6) || (ardt[1] == 9) || (ardt[1] == 11)) && (ardt[0] > 30))
        erro = true;
    else if (ardt[1] == 2) {
        if ((ardt[0] > 28) && ((ardt[2] % 4) != 0))
            erro = true;
        if ((ardt[0] > 29) && ((ardt[2] % 4) == 0))
            erro = true;
    } else if (ardt[2] < 1900)
        erro = true;
    else if (date.search(ExpReg) == -1)
        erro = true;

    if (erro)
        return false;
    else
        return true;
}

function validarData(valor) {
    var date = valor;
    var ardt = new Array;
    var ExpReg = new RegExp("(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}");
    ardt = date.split("/");
    erro = false;
    if (date == "")
        erro == false;
    else if (((ardt[1] == 4) || (ardt[1] == 6) || (ardt[1] == 9) || (ardt[1] == 11)) && (ardt[0] > 30))
        erro = true;
    else if (ardt[1] == 2) {
        if ((ardt[0] > 28) && ((ardt[2] % 4) != 0))
            erro = true;
        if ((ardt[0] > 29) && ((ardt[2] % 4) == 0))
            erro = true;
    } else if (ardt[2] < 1900)
        erro = true;
    else if (date.search(ExpReg) == -1)
        erro = true;

    if (erro)
        return false;
    else
        return true;
}

function validaDataNascimento(valor) {
    var date = valor;
    var ardt = new Array;
    ardt = date.split("/");
    erro = false;
    if (!validarData(valor))
            erro = true;
    else if (ardt[2] > 2014)
        erro = true;

    if (erro)
        return false;
    else
        return true;
}

$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############                 Geral                   ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################

    $("a.em-breve").click(function () {
        alerta("Aviso", "Em breve.");
        return false;
    });

    //$("#btnSimModal").click(function () {

    //    var id = $("#idMissao").val();

    //    window.location.href = "/Ferramenta/MissaoExcluir?id=" + id;

    //});





    $("#dsCEP").blur(function () {

        $("#loading").show();
        var cep = $(this).val().replace(/[^0-9]/, '');

        if (cep !== "") {
            $.ajax({
                type: "GET",
                url: "http://cep.republicavirtual.com.br/web_cep.php",
                data: { cep: $(this).val(), formato: "json" },
                success: function (data) {
                    debugger;
                    if (data.resultado != "0") {
                        $("#dsEndereco").val(data.tipo_logradouro + " " + data.logradouro);
                        $("#dsBairro").val(data.bairro);
                        $("#dsCidade").val(data.cidade);
                        $("#dsUF").val(data.uf);
                        $("#dsNumero").focus();

                        localStorage.setItem("CepEncontrado", "s");
                    }
                    else {
                        $("#dsLatitude").val('');
                        $("#dsLongitude").val('');
                        alert('Cep não encontrado !');
                        localStorage.setItem("CepEncontrado", "n");
                    }


                    $("#loading").hide();
                }
            });

        } else {
            $("#loading").hide();
        }

    });
    

    $("#dsNumero").blur(function () {

        $("#loading").show();
        var num = $(this).val().replace(/[^0-9]/, '');

        if (!isNaN(num)) {

            $.ajax({
                type: "GET",
                url: "https://maps.google.com/maps/api/geocode/json",
                data: { key: 'AIzaSyAkzl_k2GBB1f9uACGww_QoMdjbXTNQxLU' , address: $("#dsEndereco").val() + "+" + num + "+" + $("#dsBairro").val() + "," + $("#dsCidade").val() + $("#dsUF").val(), sensor: false, formato: "json" },
                success: function (data) {     
                    debugger;
                    try {
                        $("#dsLatitude").val(data.results[0].geometry.location.lat);
                        $("#dsLongitude").val(data.results[0].geometry.location.lng);
                    } catch (err){
                        alert('Latitude e Longitude não encontrados !');
                    }
                    if (localStorage.getItem("CepEncontrado") == 'n') {
                        $("#dsLatitude").val('');
                        $("#dsLongitude").val('');
                    }

                    $("#loading").hide();
                }
            });


        } else {
            $("#loading").hide();
        }

    });


});